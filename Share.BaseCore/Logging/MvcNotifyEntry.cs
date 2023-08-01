using AutoMapper;
using EFCore.BulkExtensions;
using Share.BaseCore.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Share.BaseCore.Logging
{

    public enum MvcNotifyType
    {
        Info,
        Success,
        Warning,
        Error
    }

    [Serializable]
    public class MvcNotifyEntry : ComparableObject<MvcNotifyEntry>
    {
        [ObjectSignature]
        public MvcNotifyType Type { get; set; }

        [ObjectSignature]
        public string Message { get; set; }

        public bool Durable { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ObjectSignatureAttribute : System.Attribute
    {
    }
    public struct HashCodeCombiner
    {
        private long _combinedHash64;

        public int CombinedHash
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _combinedHash64.GetHashCode();
        }

        public string CombinedHashString
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _combinedHash64.GetHashCode().ToString("x", CultureInfo.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private HashCodeCombiner(long seed)
        {
            _combinedHash64 = seed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashCodeCombiner Add(IEnumerable e)
        {
            if (e == null)
            {
                Add(0);
            }
            else
            {
                var count = 0;
                foreach (object o in e)
                {
                    Add(o);
                    count++;
                }
                Add(count);
            }
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(HashCodeCombiner self)
        {
            return self.CombinedHash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashCodeCombiner Add(int i)
        {
            _combinedHash64 = ((_combinedHash64 << 5) + _combinedHash64) ^ i;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashCodeCombiner Add(string s)
        {
            var hashCode = (s != null) ? s.GetHashCode() : 0;
            return Add(hashCode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashCodeCombiner Add(object o)
        {
            var hashCode = (o != null) ? o.GetHashCode() : 0;
            return Add(hashCode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashCodeCombiner Add(DateTime d)
        {
            return Add(d.GetHashCode());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashCodeCombiner Add<TValue>(TValue value, IEqualityComparer<TValue> comparer)
        {
            var hashCode = value != null ? comparer.GetHashCode(value) : 0;
            return Add(hashCode);
        }

        public HashCodeCombiner Add(FileSystemInfo fi, bool deep = true)
        {
            Guard.NotNull(fi, nameof(fi));

            if (!fi.Exists)
                return this;

            Add(fi.FullName.ToLower());
            Add(fi.CreationTimeUtc);
            Add(fi.LastWriteTimeUtc);

            if (fi is FileInfo file)
            {
                Add(file.Length.GetHashCode());
            }

            if (fi is DirectoryInfo dir)
            {
                foreach (var f in dir.GetFiles())
                {
                    Add(f);
                }
                if (deep)
                {
                    foreach (var s in dir.GetDirectories())
                    {
                        Add(s);
                    }
                }
            }

            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashCodeCombiner Start()
        {
            return new HashCodeCombiner(0x1505L);
        }
    }

    /// <summary>
    /// Provides a standard base class for facilitating sophisticated comparison of objects.
    /// </summary>
    [Serializable]
    public abstract class ComparableObject
    {
        private readonly HashSet<string> _extraSignatureProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private static readonly ConcurrentDictionary<Type, string[]> _signaturePropertyNames = new ConcurrentDictionary<Type, string[]>();

        public override bool Equals(object obj)
        {
            ComparableObject compareTo = obj as ComparableObject;

            if (ReferenceEquals(this, compareTo))
                return true;

            return compareTo != null && GetType().Equals(compareTo.GetTypeUnproxied()) &&
                HasSameSignatureAs(compareTo);
        }

        /// <summary>
        /// Used to provide the hashcode identifier of an object using the signature
        /// properties of the object; Since it is recommended that GetHashCode change infrequently,
        /// if at all, in an object's lifetime; it's important that properties are carefully
        /// selected which truly represent the signature of an object.
        /// </summary>
        [SuppressMessage("ReSharper", "BaseObjectGetHashCodeCallInGetHashCode")]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public override int GetHashCode()
        {
            unchecked
            {
                var signatureProperties = GetSignatureProperties().ToArray();
                Type t = this.GetType();

                var combiner = HashCodeCombiner.Start();

                // It's possible for two objects to return the same hash code based on
                // identically valued properties, even if they're of two different types,
                // so we include the object's type in the hash calculation
                combiner.Add(t.GetHashCode());

                foreach (var prop in signatureProperties)
                {
                    var value = prop.GetValue(this);

                    if (value != null)
                        combiner.Add(value.GetHashCode());
                }

                if (signatureProperties.Length > 0)
                    return combiner.CombinedHash;

                // If no properties were flagged as being part of the signature of the object,
                // then simply return the hashcode of the base object as the hashcode.
                return base.GetHashCode();
            }
        }

        /// <summary>
        /// Returns the real underlying type of proxied objects.
        /// </summary>
        protected virtual Type GetTypeUnproxied()
        {
            return this.GetType();
        }

        /// <summary>
        /// You may override this method to provide your own comparison routine.
        /// </summary>
        protected virtual bool HasSameSignatureAs(ComparableObject compareTo)
        {
            if (compareTo == null)
                return false;

            var signatureProperties = GetSignatureProperties();

            foreach (var pi in signatureProperties)
            {
                object thisValue = pi.GetValue(this);
                object thatValue = pi.GetValue(compareTo);

                if (thisValue == null && thatValue == null)
                    continue;

                if ((thisValue == null ^ thatValue == null) ||
                    (!thisValue.Equals(thatValue)))
                {
                    return false;
                }
            }

            // If we've gotten this far and signature properties were found, then we can
            // assume that everything matched; otherwise, if there were no signature
            // properties, then simply return the default bahavior of Equals
            return signatureProperties.Any() || base.Equals(compareTo);
        }

        /// <summary>
        /// </summary>
        public IEnumerable<FastProperty> GetSignatureProperties()
        {
            var type = GetType();
            var propertyNames = GetSignaturePropertyNamesCore();

            foreach (var name in propertyNames)
            {
                var fastProperty = FastProperty.GetProperty(type, name);
                if (fastProperty != null)
                {
                    yield return fastProperty;
                }
            }
        }

        /// <summary>
        /// Enforces the template method pattern to have child objects determine which specific
        /// properties should and should not be included in the object signature comparison.
        /// </summary>
        protected virtual string[] GetSignaturePropertyNamesCore()
        {
            Type type = this.GetType();
            string[] names;

            if (!_signaturePropertyNames.TryGetValue(type, out names))
            {
                names = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => System.Attribute.IsDefined(p, typeof(ObjectSignatureAttribute), true))
                    .Select(p => p.Name)
                    .ToArray();

                _signaturePropertyNames.TryAdd(type, names);
            }

            if (_extraSignatureProperties.Count == 0)
            {
                return names;
            }

            return names.Union(_extraSignatureProperties).ToArray();
        }

        /// <summary>
        /// Adds an extra property to the type specific signature properties list.
        /// </summary>
        /// <param name="propertyName">Name of the property to add.</param>
        /// <remarks>Both lists are <c>unioned</c>, so
        /// that no duplicates can occur within the global descriptor collection.</remarks>
        protected void RegisterSignatureProperty(string propertyName)
        {
            Guard.NotEmpty(propertyName, nameof(propertyName));

            _extraSignatureProperties.Add(propertyName);
        }

    }

    /// <summary>
    /// Generic version of <see cref="ComparableObject" />.
    /// </summary>
	[Serializable]
    public abstract class ComparableObject<T> : ComparableObject, IEquatable<T>
    {
        /// <summary>
        /// Adds an extra property to the type specific signature properties list.
        /// </summary>
        /// <param name="expression">The lambda expression for the property to add.</param>
        /// <remarks>Both lists are <c>unioned</c>, so
        /// that no duplicates can occur within the global descriptor collection.</remarks>
        protected void RegisterSignatureProperty(Expression<Func<T, object>> expression)
        {
            Guard.NotNull(expression, nameof(expression));

            base.RegisterSignatureProperty(expression.ExtractPropertyInfo().Name);
        }

        public virtual bool Equals(T other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return base.Equals(other);
        }
    }


    public enum PropertyCachingStrategy
    {
        /// <summary>
        /// Don't cache FastProperty instances
        /// </summary>
        Uncached = 0,
        /// <summary>
        /// Always cache FastProperty instances
        /// </summary>
        Cached = 1,
        /// <summary>
        /// Always cache FastProperty instances. PLUS cache all other properties of the declaring type.
        /// </summary>
        EagerCached = 2
    }

    public abstract class FastProperty
    {
        private static readonly ConcurrentDictionary<PropertyKey, FastProperty> _singlePropertiesCache = new ConcurrentDictionary<PropertyKey, FastProperty>();

        // Using an array rather than IEnumerable, as target will be called on the hot path numerous times.
        private static readonly ConcurrentDictionary<Type, IDictionary<string, FastProperty>> _propertiesCache = new ConcurrentDictionary<Type, IDictionary<string, FastProperty>>();
        private static readonly ConcurrentDictionary<Type, IDictionary<string, FastProperty>> _visiblePropertiesCache = new ConcurrentDictionary<Type, IDictionary<string, FastProperty>>();

        private Func<object, object> _valueGetter;
        private Action<object, object> _valueSetter;
        private bool? _isPublicSettable;
        private bool? _isSequenceType;

        /// <summary>
        /// Initializes a <see cref="FastProperty"/>.
        /// This constructor does not cache the helper. For caching, use <see cref="GetProperties(object, PropertyCachingStrategy)"/>.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberCallInContructor")]
        protected FastProperty(PropertyInfo property)
        {
            Guard.NotNull(property, nameof(property));

            Property = property;
            Name = property.Name;
        }

        /// <summary>
        /// Gets the property value getter.
        /// </summary>
        public Func<object, object> ValueGetter
        {
            get
            {
                if (_valueGetter == null)
                {
                    // We'll allow safe races here.
                    _valueGetter = MakePropertyGetter(Property);
                }

                return _valueGetter;
            }
            private set => _valueGetter = value;
        }

        /// <summary>
        /// Gets the property value setter.
        /// </summary>
        public Action<object, object> ValueSetter
        {
            get
            {
                if (_valueSetter == null)
                {
                    // We'll allow safe races here.
                    _valueSetter = MakePropertySetter(Property);
                }

                return _valueSetter;
            }
            private set => _valueSetter = value;
        }

        protected abstract Func<object, object> MakePropertyGetter(PropertyInfo propertyInfo);

        protected abstract Action<object, object> MakePropertySetter(PropertyInfo propertyInfo);

        /// <summary>
        /// Gets the backing <see cref="PropertyInfo"/>.
        /// </summary>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// Gets (or sets in derived types) the property name.
        /// </summary>
        public virtual string Name { get; protected set; }

        public bool IsPublicSettable
        {
            get
            {
                if (!_isPublicSettable.HasValue)
                {
                    _isPublicSettable = Property.CanWrite && Property.GetSetMethod(false) != null;
                }
                return _isPublicSettable.Value;
            }
        }

        public bool IsSequenceType
        {
            get
            {
                if (!_isSequenceType.HasValue)
                {
                    _isSequenceType = Property.PropertyType != typeof(string) && Property.PropertyType.IsSubClass(typeof(IEnumerable<>));
                }
                return _isSequenceType.Value;
            }
        }

        /// <summary>
        /// Returns the property value for the specified <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The object whose property value will be returned.</param>
        /// <returns>The property value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetValue(object instance)
        {
            return ValueGetter(instance);
        }

        /// <summary>
        /// Sets the property value for the specified <paramref name="instance" />.
        /// </summary>
        /// <param name="instance">The object whose property value will be set.</param>
        /// <param name="value">The property value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(object instance, object value)
        {
            ValueSetter(instance, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FastProperty GetProperty<T>(
            Expression<Func<T, object>> property,
            PropertyCachingStrategy cachingStrategy = PropertyCachingStrategy.Cached)
        {
            return GetProperty(property.ExtractPropertyInfo(), cachingStrategy);
        }

        public static FastProperty GetProperty(
            Type type,
            string propertyName,
            PropertyCachingStrategy cachingStrategy = PropertyCachingStrategy.Cached)
        {
            Guard.NotNull(type, nameof(type));
            Guard.NotEmpty(propertyName, nameof(propertyName));

            if (TryGetCachedProperty(type, propertyName, cachingStrategy == PropertyCachingStrategy.EagerCached, out var fastProperty))
            {
                return fastProperty;
            }

            var key = new PropertyKey(type, propertyName);
            if (!_singlePropertiesCache.TryGetValue(key, out fastProperty))
            {
                var pi = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (pi != null)
                {
                    fastProperty = Create(pi);
                    if (cachingStrategy > PropertyCachingStrategy.Uncached)
                    {
                        _singlePropertiesCache.TryAdd(key, fastProperty);
                    }
                }
            }

            return fastProperty;
        }

        public static FastProperty GetProperty(
            PropertyInfo propertyInfo,
            PropertyCachingStrategy cachingStrategy = PropertyCachingStrategy.Cached)
        {
            Guard.NotNull(propertyInfo, nameof(propertyInfo));

            if (TryGetCachedProperty(propertyInfo.ReflectedType, propertyInfo.Name, cachingStrategy == PropertyCachingStrategy.EagerCached, out var fastProperty))
            {
                return fastProperty;
            }

            var key = new PropertyKey(propertyInfo.ReflectedType, propertyInfo.Name);
            if (!_singlePropertiesCache.TryGetValue(key, out fastProperty))
            {
                fastProperty = Create(propertyInfo);
                if (cachingStrategy > PropertyCachingStrategy.Uncached)
                {
                    _singlePropertiesCache.TryAdd(key, fastProperty);
                }
            }

            return fastProperty;
        }

        private static bool TryGetCachedProperty(
            Type type,
            string propertyName,
            bool eagerCached,
            out FastProperty fastProperty)
        {
            fastProperty = null;
            IDictionary<string, FastProperty> allProperties;

            if (eagerCached)
            {
                allProperties = (IDictionary<string, FastProperty>)GetProperties(type);
                allProperties.TryGetValue(propertyName, out fastProperty);
            }

            if (fastProperty == null && _propertiesCache.TryGetValue(type, out allProperties))
            {
                allProperties.TryGetValue(propertyName, out fastProperty);
            }

            return fastProperty != null;
        }

        ///  <summary>
        ///  Given an object, adds each instance property with a public get method as a key and its
        ///  associated value to a dictionary.
        /// 
        ///  If the object is already an <see>
        ///          <cref>IDictionary{string, object}</cref>
        ///      </see>
        ///      instance, then a copy
        ///  is returned.
        ///  </summary>
        ///  <param name="keySelector">Key selector</param>
        ///  <param name="deep">When true, converts all nested objects to dictionaries also</param>
        ///  <remarks>
        ///  The implementation of FastProperty will cache the property accessors per-type. This is
        ///  faster when the the same type is used multiple times with ObjectToDictionary.
        ///  </remarks>
        public static IDictionary<string, object> ObjectToDictionary(object value, Func<string, string> keySelector = null, bool deep = false)
        {
            if (value is IDictionary<string, object> dictionary)
            {
                return new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
            }

            dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (value != null)
            {
                keySelector = keySelector ?? new Func<string, string>(key => key);

                foreach (var prop in GetProperties(value.GetType()).Values)
                {
                    var propValue = prop.GetValue(value);
                    if (deep && propValue != null && prop.Property.PropertyType.IsPlainObjectType())
                    {
                        propValue = ObjectToDictionary(propValue, deep: true);
                    }

                    dictionary[keySelector(prop.Name)] = propValue;
                }
            }

            return dictionary;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FastProperty Create(PropertyInfo property)
        {
            return new DelegatedAccessor(property);
        }


        /// <summary>
        /// <para>
        /// Creates and caches fast property helpers that expose getters for every non-hidden get property
        /// on the specified type.
        /// </para>
        /// <para>
        /// <see cref="GetVisibleProperties(Type, PropertyCachingStrategy)"/> excludes properties defined on base types that have been
        /// hidden by definitions using the <c>new</c> keyword.
        /// </para>
        /// </summary>
        /// <param name="type">The type to extract property accessors for.</param>
        /// <returns>
        /// A cached array of all public property getters from the type.
        /// </returns>
        public static IReadOnlyDictionary<string, FastProperty> GetVisibleProperties(Type type, PropertyCachingStrategy cachingStrategy = PropertyCachingStrategy.Cached)
        {
            Guard.NotNull(type, nameof(type));

            // Unwrap nullable types. This means Nullable<T>.Value and Nullable<T>.HasValue will not be
            // part of the sequence of properties returned by this method.
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (_visiblePropertiesCache.TryGetValue(type, out var result))
            {
                return (IReadOnlyDictionary<string, FastProperty>)result;
            }

            var visiblePropertiesCache = cachingStrategy > PropertyCachingStrategy.Uncached ? _visiblePropertiesCache : CreateVolatileCache();

            // The simple and common case, this is normal POCO object - no need to allocate.
            var allPropertiesDefinedOnType = true;
            var allProperties = GetProperties(type, cachingStrategy);
            foreach (var prop in allProperties.Values)
            {
                if (prop.Property.DeclaringType != type)
                {
                    allPropertiesDefinedOnType = false;
                    break;
                }
            }

            if (allPropertiesDefinedOnType)
            {
                result = (IDictionary<string, FastProperty>)allProperties;
                visiblePropertiesCache.TryAdd(type, result);
                return allProperties;
            }

            // There's some inherited properties here, so we need to check for hiding via 'new'.
            var filteredProperties = new List<FastProperty>(allProperties.Count);
            foreach (var prop in allProperties.Values)
            {
                var declaringType = prop.Property.DeclaringType;
                if (declaringType == type)
                {
                    filteredProperties.Add(prop);
                    continue;
                }

                // If this property was declared on a base type then look for the definition closest to the
                // the type to see if we should include it.
                var ignoreProperty = false;

                // Walk up the hierarchy until we find the type that actally declares this
                // PropertyInfo.
                var currentTypeInfo = type.GetTypeInfo();
                var declaringTypeInfo = declaringType.GetTypeInfo();
                while (currentTypeInfo != null && currentTypeInfo != declaringTypeInfo)
                {
                    // We've found a 'more proximal' public definition
                    var declaredProperty = currentTypeInfo.GetDeclaredProperty(prop.Name);
                    if (declaredProperty != null)
                    {
                        ignoreProperty = true;
                        break;
                    }

                    if (currentTypeInfo.BaseType != null)
                    {
                        currentTypeInfo = currentTypeInfo.BaseType.GetTypeInfo();
                    }

                }

                if (!ignoreProperty)
                {
                    filteredProperties.Add(prop);
                }
            }

            result = filteredProperties.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
            visiblePropertiesCache.TryAdd(type, result);
            return (IReadOnlyDictionary<string, FastProperty>)result;
        }

        /// <summary>
        /// Creates and caches fast property helpers that expose getters for every public get property on the
        /// specified type.
        /// </summary>
        /// <param name="type">The type to extract property accessors for.</param>
        /// <returns>A cached array of all public property getters from the type of target instance.
        /// </returns>
        public static IReadOnlyDictionary<string, FastProperty> GetProperties(Type type, PropertyCachingStrategy cachingStrategy = PropertyCachingStrategy.Cached)
        {
            Guard.NotNull(type, nameof(type));

            // Unwrap nullable types. This means Nullable<T>.Value and Nullable<T>.HasValue will not be
            // part of the sequence of properties returned by this method.
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (!_propertiesCache.TryGetValue(type, out var props))
            {
                if (cachingStrategy == PropertyCachingStrategy.Uncached)
                {
                    props = Get(type);
                }
                else
                {
                    props = _propertiesCache.GetOrAdd(type, Get);
                }
            }

            return (IReadOnlyDictionary<string, FastProperty>)props;

            IDictionary<string, FastProperty> Get(Type t)
            {
                var candidates = GetCandidateProperties(t);
                var fastProperties = candidates.Select(p => Create(p)).ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
                CleanDuplicates(t, fastProperties.Keys);
                return fastProperties;
            }
        }

        private static void CleanDuplicates(Type type, IEnumerable<string> propNames)
        {
            foreach (var name in propNames)
            {
                var key = new PropertyKey(type, name);
                _singlePropertiesCache.TryRemove(key, out _);
            }
        }

        internal static IEnumerable<PropertyInfo> GetCandidateProperties(Type type)
        {
            // We avoid loading indexed properties using the Where statement.
            var properties = type.GetRuntimeProperties().Where(IsCandidateProperty);

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsInterface)
            {
                // Reflection does not return information about inherited properties on the interface itself.
                properties = properties.Concat(typeInfo.ImplementedInterfaces.SelectMany(
                    interfaceType => interfaceType.GetRuntimeProperties().Where(IsCandidateProperty)));
            }

            return properties;
        }

        // Indexed properties are not useful (or valid) for grabbing properties off an object.
        private static bool IsCandidateProperty(PropertyInfo property)
        {
            return property.GetIndexParameters().Length == 0 &&
                property.GetMethod != null &&
                property.GetMethod.IsPublic &&
                !property.GetMethod.IsStatic;
        }

        private static ConcurrentDictionary<Type, IDictionary<string, FastProperty>> CreateVolatileCache()
        {
            return new ConcurrentDictionary<Type, IDictionary<string, FastProperty>>();
        }

        class PropertyKey : Tuple<Type, string>
        {
            public PropertyKey(Type type, string propertyName)
                : base(type, propertyName)
            {
            }
            public Type Type => base.Item1;
            public string PropertyName => base.Item2;
        }
    }



    [DebuggerDisplay("DelegateAccessor: {Name}")]
    internal sealed class DelegatedAccessor : FastProperty
    {
        // Delegate type for a by-ref property getter
        private delegate TValue ByRefFunc<TDeclaringType, TValue>(ref TDeclaringType arg);

        private static readonly MethodInfo CallPropertyGetterOpenGenericMethod = typeof(DelegatedAccessor).GetTypeInfo().GetDeclaredMethod("CallPropertyGetter");
        private static readonly MethodInfo CallPropertyGetterByReferenceOpenGenericMethod = typeof(DelegatedAccessor).GetTypeInfo().GetDeclaredMethod("CallPropertyGetterByReference");
        private static readonly MethodInfo CallNullSafePropertyGetterOpenGenericMethod = typeof(DelegatedAccessor).GetTypeInfo().GetDeclaredMethod("CallNullSafePropertyGetter");
        private static readonly MethodInfo CallNullSafePropertyGetterByReferenceOpenGenericMethod = typeof(DelegatedAccessor).GetTypeInfo().GetDeclaredMethod("CallNullSafePropertyGetterByReference");
        private static readonly MethodInfo CallPropertySetterOpenGenericMethod = typeof(DelegatedAccessor).GetTypeInfo().GetDeclaredMethod("CallPropertySetter");

        public DelegatedAccessor(PropertyInfo property)
            : base(property)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Func<object, object> MakePropertyGetter(PropertyInfo propertyInfo)
        {
            Debug.Assert(propertyInfo != null);

            return MakeFastPropertyGetter(
                propertyInfo,
                CallPropertyGetterOpenGenericMethod,
                CallPropertyGetterByReferenceOpenGenericMethod);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Action<object, object> MakePropertySetter(PropertyInfo propertyInfo)
        {
            Debug.Assert(propertyInfo != null);
            Debug.Assert(!propertyInfo.DeclaringType.GetTypeInfo().IsValueType);

            var setMethod = propertyInfo.SetMethod;
            Debug.Assert(setMethod != null);
            Debug.Assert(!setMethod.IsStatic);
            Debug.Assert(setMethod.ReturnType == typeof(void));
            var parameters = setMethod.GetParameters();
            Debug.Assert(parameters.Length == 1);

            // Instance methods in the CLR can be turned into static methods where the first parameter
            // is open over "target". This parameter is always passed by reference, so we have a code
            // path for value types and a code path for reference types.
            var typeInput = setMethod.DeclaringType;
            var parameterType = parameters[0].ParameterType;

            // Create a delegate TDeclaringType -> { TDeclaringType.Property = TValue; }
            var propertySetterAsAction =
                setMethod.CreateDelegate(typeof(Action<,>).MakeGenericType(typeInput, parameterType));
            var callPropertySetterClosedGenericMethod =
                CallPropertySetterOpenGenericMethod.MakeGenericMethod(typeInput, parameterType);
            var callPropertySetterDelegate =
                callPropertySetterClosedGenericMethod.CreateDelegate(
                    typeof(Action<object, object>), propertySetterAsAction);

            return (Action<object, object>)callPropertySetterDelegate;
        }

        /// <summary>
        /// Creates a single fast property getter which is safe for a null input object. The result is not cached.
        /// </summary>
        /// <param name="propertyInfo">propertyInfo to extract the getter for.</param>
        /// <returns>A fast getter.</returns>
        /// <remarks>
        /// This method is more memory efficient than a dynamically compiled lambda, and about the
        /// same speed.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Func<object, object> MakeNullSafeFastPropertyGetter(PropertyInfo propertyInfo)
        {
            Debug.Assert(propertyInfo != null);

            return MakeFastPropertyGetter(
                propertyInfo,
                CallNullSafePropertyGetterOpenGenericMethod,
                CallNullSafePropertyGetterByReferenceOpenGenericMethod);
        }

        static Func<object, object> MakeFastPropertyGetter(
            PropertyInfo propertyInfo,
            MethodInfo propertyGetterWrapperMethod,
            MethodInfo propertyGetterByRefWrapperMethod)
        {
            Debug.Assert(propertyInfo != null);

            // Must be a generic method with a Func<,> parameter
            Debug.Assert(propertyGetterWrapperMethod != null);
            Debug.Assert(propertyGetterWrapperMethod.IsGenericMethodDefinition);
            Debug.Assert(propertyGetterWrapperMethod.GetParameters().Length == 2);

            // Must be a generic method with a ByRefFunc<,> parameter
            Debug.Assert(propertyGetterByRefWrapperMethod != null);
            Debug.Assert(propertyGetterByRefWrapperMethod.IsGenericMethodDefinition);
            Debug.Assert(propertyGetterByRefWrapperMethod.GetParameters().Length == 2);

            var getMethod = propertyInfo.GetMethod;
            Debug.Assert(getMethod != null);
            Debug.Assert(!getMethod.IsStatic);
            Debug.Assert(getMethod.GetParameters().Length == 0);

            // Instance methods in the CLR can be turned into static methods where the first parameter
            // is open over "target". This parameter is always passed by reference, so we have a code
            // path for value types and a code path for reference types.
            if (getMethod.DeclaringType.GetTypeInfo().IsValueType)
            {
                // Create a delegate (ref TDeclaringType) -> TValue
                return MakeFastPropertyGetter(
                    typeof(ByRefFunc<,>),
                    getMethod,
                    propertyGetterByRefWrapperMethod);
            }
            else
            {
                // Create a delegate TDeclaringType -> TValue
                return MakeFastPropertyGetter(
                    typeof(Func<,>),
                    getMethod,
                    propertyGetterWrapperMethod);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Func<object, object> MakeFastPropertyGetter(
            Type openGenericDelegateType,
            MethodInfo propertyGetMethod,
            MethodInfo openGenericWrapperMethod)
        {
            var typeInput = propertyGetMethod.DeclaringType;
            var typeOutput = propertyGetMethod.ReturnType;

            var delegateType = openGenericDelegateType.MakeGenericType(typeInput, typeOutput);
            var propertyGetterDelegate = propertyGetMethod.CreateDelegate(delegateType);

            var wrapperDelegateMethod = openGenericWrapperMethod.MakeGenericMethod(typeInput, typeOutput);
            var accessorDelegate = wrapperDelegateMethod.CreateDelegate(
                typeof(Func<object, object>),
                propertyGetterDelegate);

            return (Func<object, object>)accessorDelegate;
        }

        // Called via reflection
        private static object CallPropertyGetter<TDeclaringType, TValue>(
            Func<TDeclaringType, TValue> getter,
            object target)
        {
            return getter((TDeclaringType)target);
        }

        // Called via reflection
        private static object CallPropertyGetterByReference<TDeclaringType, TValue>(
            ByRefFunc<TDeclaringType, TValue> getter,
            object target)
        {
            var unboxed = (TDeclaringType)target;
            return getter(ref unboxed);
        }

        // Called via reflection
        private static object CallNullSafePropertyGetter<TDeclaringType, TValue>(
            Func<TDeclaringType, TValue> getter,
            object target)
        {
            if (target == null)
            {
                return null;
            }

            return getter((TDeclaringType)target);
        }

        // Called via reflection
        private static object CallNullSafePropertyGetterByReference<TDeclaringType, TValue>(
            ByRefFunc<TDeclaringType, TValue> getter,
            object target)
        {
            if (target == null)
            {
                return null;
            }

            var unboxed = (TDeclaringType)target;
            return getter(ref unboxed);
        }

        private static void CallPropertySetter<TDeclaringType, TValue>(
            Action<TDeclaringType, TValue> setter,
            object target,
            object value)
        {
            setter((TDeclaringType)target, (TValue)value);
        }
    }
}