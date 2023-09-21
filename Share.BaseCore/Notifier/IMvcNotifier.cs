using AutoMapper;
using EFCore.BulkExtensions;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Share.BaseCore.Base;
using Share.BaseCore.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Share.BaseCore.Notifier
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class MvcNotifyAttribute : ActionFilterAttribute
    {
        public const string NotificationsKey = "base.notifications.all";

        // Tạm comment lại vì đang không tự Property Injection được,
        // mà khi Resolve ở ctor thì không có Entries => Resolve tại scope của Action
        //public IMvcNotifier MvcNotifier { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var mvcNotifier = EngineContext.Current.Resolve<IMvcNotifier>();

            if (mvcNotifier == null || !mvcNotifier.Entries.Any())
                return;

            var controller = (Controller)filterContext.Controller;

            PersistViewData(controller.ViewData, mvcNotifier.Entries.Where(x => x.Durable == false));
            PersistTempData(controller.TempData, mvcNotifier.Entries.Where(x => x.Durable == true));

            mvcNotifier.Entries.Clear();
        }

        private void PersistViewData(ViewDataDictionary viewData, IEnumerable<MvcNotifyEntry> source)
        {
            if (!source.Any())
                return;

            var existing = (viewData[NotificationsKey] ?? new HashSet<MvcNotifyEntry>()) as HashSet<MvcNotifyEntry>;

            source.Each(x =>
            {
                if (x.Message.HasValue())
                    existing.Add(x);
            });

            viewData[NotificationsKey] = TrimSet(existing);
        }

        private void PersistTempData(ITempDataDictionary tempData, IEnumerable<MvcNotifyEntry> source)
        {
            if (!source.Any())
                return;

            //var existing = (bag[NotificationsKey] ?? new HashSet<MvcNotifyEntry>()) as HashSet<MvcNotifyEntry>;
            var existing = new HashSet<MvcNotifyEntry>();
            if (tempData.ContainsKey(NotificationsKey))
                existing = tempData.Get<HashSet<MvcNotifyEntry>>(NotificationsKey);

            source.Each(x =>
            {
                if (x.Message.HasValue())
                    existing.Add(x);
            });

            //tempData[NotificationsKey] = TrimSet(existing);
            tempData.Put(NotificationsKey, TrimSet(existing));
        }

        private void HandleAjaxRequest(MvcNotifyEntry entry, HttpResponse response)
        {
            if (entry == null)
                return;

            response.Headers.Add("X-XBase-Message-Type", entry.Type.ToString().ToLower());
            response.Headers.Add("X-XBase-Message", Convert.ToBase64String(Encoding.UTF8.GetBytes(entry.Message)));
        }

        private HashSet<MvcNotifyEntry> TrimSet(HashSet<MvcNotifyEntry> entries)
        {
            if (entries.Count <= 20)
            {
                return entries;
            }

            return new HashSet<MvcNotifyEntry>(entries.Skip(entries.Count - 20));
        }
    }

    public enum PropertyCachingStrategy
    {
        //
        // Summary:
        //     Don't cache FastProperty instances
        Uncached,
        //
        // Summary:
        //     Always cache FastProperty instances
        Cached,
        //
        // Summary:
        //     Always cache FastProperty instances. PLUS cache all other properties of the declaring
        //     type.
        EagerCached
    }
    public enum MvcNotifyType
    {
        Info,
        Success,
        Warning,
        Error
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ObjectSignatureAttribute :System.Attribute
    {
    }
    public interface IMvcNotifier
    {
        ICollection<MvcNotifyEntry> Entries { get; }

        void Add(MvcNotifyType type, string message, bool durable = true);
    }
    public class MvcNotifier : IMvcNotifier
    {
        private readonly HashSet<MvcNotifyEntry> _entries = new HashSet<MvcNotifyEntry>();

        public ICollection<MvcNotifyEntry> Entries => _entries;

        public void Add(MvcNotifyType type, string message, bool durable = true)
        {
            _entries.Add(new MvcNotifyEntry
            {
                Type = type,
                Message = message,
                Durable = durable
            });
        }
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

    //
    // Summary:
    //     Generic version of VTQT.ComparableObject.
    [Serializable]
    public abstract class ComparableObject<T> : ComparableObject, IEquatable<T>
    {
        //
        // Summary:
        //     Adds an extra property to the type specific signature properties list.
        //
        // Parameters:
        //   expression:
        //     The lambda expression for the property to add.
        //
        // Remarks:
        //     Both lists are unioned, so that no duplicates can occur within the global descriptor
        //     collection.
        protected void RegisterSignatureProperty(Expression<Func<T, object>> expression)
        {
            Guard.NotNull(expression, "expression");
            RegisterSignatureProperty(expression.ExtractPropertyInfo().Name);
        }

        public virtual bool Equals(T other)
        {
            if (other == null)
            {
                return false;
            }

            if (this == (object)other)
            {
                return true;
            }

            return Equals((object)other);
        }
    }

    //
    // Summary:
    //     Provides a standard base class for facilitating sophisticated comparison of objects.
    [Serializable]
    public abstract class ComparableObject
    {
        private readonly HashSet<string> _extraSignatureProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private static readonly ConcurrentDictionary<Type, string[]> _signaturePropertyNames = new ConcurrentDictionary<Type, string[]>();

        public override bool Equals(object obj)
        {
            ComparableObject comparableObject = obj as ComparableObject;
            if (this == comparableObject)
            {
                return true;
            }

            return comparableObject != null && GetType().Equals(comparableObject.GetTypeUnproxied()) && HasSameSignatureAs(comparableObject);
        }

        //
        // Summary:
        //     Used to provide the hashcode identifier of an object using the signature properties
        //     of the object; Since it is recommended that GetHashCode change infrequently,
        //     if at all, in an object's lifetime; it's important that properties are carefully
        //     selected which truly represent the signature of an object.
        public override int GetHashCode()
        {
            FastProperty[] array = GetSignatureProperties().ToArray();
            Type type = GetType();
            HashCodeCombiner hashCodeCombiner = HashCodeCombiner.Start();
            hashCodeCombiner.Add(type.GetHashCode());
            FastProperty[] array2 = array;
            foreach (FastProperty fastProperty in array2)
            {
                object value = fastProperty.GetValue(this);
                if (value != null)
                {
                    hashCodeCombiner.Add(value.GetHashCode());
                }
            }

            if (array.Length != 0)
            {
                return hashCodeCombiner.CombinedHash;
            }

            return base.GetHashCode();
        }

        //
        // Summary:
        //     Returns the real underlying type of proxied objects.
        protected virtual Type GetTypeUnproxied()
        {
            return GetType();
        }

        //
        // Summary:
        //     You may override this method to provide your own comparison routine.
        protected virtual bool HasSameSignatureAs(ComparableObject compareTo)
        {
            if (compareTo == null)
            {
                return false;
            }

            IEnumerable<FastProperty> signatureProperties = GetSignatureProperties();
            foreach (FastProperty item in signatureProperties)
            {
                object value = item.GetValue(this);
                object value2 = item.GetValue(compareTo);
                if ((value == null && value2 == null) || (!((value == null) ^ (value2 == null)) && value.Equals(value2)))
                {
                    continue;
                }

                return false;
            }

            return signatureProperties.Any() || base.Equals(compareTo);
        }

        public IEnumerable<FastProperty> GetSignatureProperties()
        {
            Type type = GetType();
            string[] propertyNames = GetSignaturePropertyNamesCore();
            string[] array = propertyNames;
            foreach (string name in array)
            {
                FastProperty fastProperty = FastProperty.GetProperty(type, name);
                if (fastProperty != null)
                {
                    yield return fastProperty;
                }
            }
        }

        //
        // Summary:
        //     Enforces the template method pattern to have child objects determine which specific
        //     properties should and should not be included in the object signature comparison.
        protected virtual string[] GetSignaturePropertyNamesCore()
        {
            Type type = GetType();
            if (!_signaturePropertyNames.TryGetValue(type, out var value))
            {
                value = (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                         where System.Attribute.IsDefined(p, typeof(ObjectSignatureAttribute), inherit: true)
                         select p.Name).ToArray();
                _signaturePropertyNames.TryAdd(type, value);
            }

            if (_extraSignatureProperties.Count == 0)
            {
                return value;
            }

            return value.Union(_extraSignatureProperties).ToArray();
        }

        //
        // Summary:
        //     Adds an extra property to the type specific signature properties list.
        //
        // Parameters:
        //   propertyName:
        //     Name of the property to add.
        //
        // Remarks:
        //     Both lists are unioned, so that no duplicates can occur within the global descriptor
        //     collection.
        protected void RegisterSignatureProperty(string propertyName)
        {
            Guard.NotEmpty(propertyName, "propertyName");
            _extraSignatureProperties.Add(propertyName);
        }
    }
    public struct HashCodeCombiner
    {
        private long _combinedHash64;

        public int CombinedHash
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _combinedHash64.GetHashCode();
            }
        }

        public string CombinedHashString
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _combinedHash64.GetHashCode().ToString("x", CultureInfo.InvariantCulture);
            }
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
                int num = 0;
                foreach (object? item in e)
                {
                    Add(item);
                    num++;
                }

                Add(num);
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
            int i = s?.GetHashCode() ?? 0;
            return Add(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashCodeCombiner Add(object o)
        {
            int i = o?.GetHashCode() ?? 0;
            return Add(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashCodeCombiner Add(DateTime d)
        {
            return Add(d.GetHashCode());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashCodeCombiner Add<TValue>(TValue value, IEqualityComparer<TValue> comparer)
        {
            int i = ((value != null) ? comparer.GetHashCode(value) : 0);
            return Add(i);
        }

        public HashCodeCombiner Add(FileSystemInfo fi, bool deep = true)
        {
            Guard.NotNull(fi, "fi");
            if (!fi.Exists)
            {
                return this;
            }

            Add(fi.FullName.ToLower());
            Add(fi.CreationTimeUtc);
            Add(fi.LastWriteTimeUtc);
            FileInfo fileInfo = fi as FileInfo;
            if (fileInfo != null)
            {
                Add(fileInfo.Length.GetHashCode());
            }

            DirectoryInfo directoryInfo = fi as DirectoryInfo;
            if (directoryInfo != null)
            {
                FileInfo[] files = directoryInfo.GetFiles();
                foreach (FileInfo fi2 in files)
                {
                    Add(fi2);
                }

                if (deep)
                {
                    DirectoryInfo[] directories = directoryInfo.GetDirectories();
                    foreach (DirectoryInfo fi3 in directories)
                    {
                        Add(fi3);
                    }
                }
            }

            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashCodeCombiner Start()
        {
            return new HashCodeCombiner(5381L);
        }
    }


    public abstract class FastProperty
    {
        private class PropertyKey : Tuple<Type, string>
        {
            public Type Type => base.Item1;

            public string PropertyName => base.Item2;

            public PropertyKey(Type type, string propertyName)
                : base(type, propertyName)
            {
            }
        }

        private static readonly ConcurrentDictionary<PropertyKey, FastProperty> _singlePropertiesCache = new ConcurrentDictionary<PropertyKey, FastProperty>();

        private static readonly ConcurrentDictionary<Type, IDictionary<string, FastProperty>> _propertiesCache = new ConcurrentDictionary<Type, IDictionary<string, FastProperty>>();

        private static readonly ConcurrentDictionary<Type, IDictionary<string, FastProperty>> _visiblePropertiesCache = new ConcurrentDictionary<Type, IDictionary<string, FastProperty>>();

        private Func<object, object> _valueGetter;

        private Action<object, object> _valueSetter;

        private bool? _isPublicSettable;

        private bool? _isSequenceType;

        //
        // Summary:
        //     Gets the property value getter.
        public Func<object, object> ValueGetter
        {
            get
            {
                if (_valueGetter == null)
                {
                    _valueGetter = MakePropertyGetter(Property);
                }

                return _valueGetter;
            }
            private set
            {
                _valueGetter = value;
            }
        }

        //
        // Summary:
        //     Gets the property value setter.
        public Action<object, object> ValueSetter
        {
            get
            {
                if (_valueSetter == null)
                {
                    _valueSetter = MakePropertySetter(Property);
                }

                return _valueSetter;
            }
            private set
            {
                _valueSetter = value;
            }
        }

        //
        // Summary:
        //     Gets the backing System.Reflection.PropertyInfo.
        public PropertyInfo Property { get; private set; }

        //
        // Summary:
        //     Gets (or sets in derived types) the property name.
        public virtual string Name { get; protected set; }

        public bool IsPublicSettable
        {
            get
            {
                if (!_isPublicSettable.HasValue)
                {
                    _isPublicSettable = Property.CanWrite && Property.GetSetMethod(nonPublic: false) != null;
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

        //
        // Summary:
        //     Initializes a VTQT.ComponentModel.FastProperty. This constructor does not cache
        //     the helper. For caching, use GetProperties(object, PropertyCachingStrategy).
        protected FastProperty(PropertyInfo property)
        {
            Guard.NotNull(property, "property");
            Property = property;
            Name = property.Name;
        }

        protected abstract Func<object, object> MakePropertyGetter(PropertyInfo propertyInfo);

        protected abstract Action<object, object> MakePropertySetter(PropertyInfo propertyInfo);

        //
        // Summary:
        //     Returns the property value for the specified instance.
        //
        // Parameters:
        //   instance:
        //     The object whose property value will be returned.
        //
        // Returns:
        //     The property value.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetValue(object instance)
        {
            return ValueGetter(instance);
        }

        //
        // Summary:
        //     Sets the property value for the specified instance.
        //
        // Parameters:
        //   instance:
        //     The object whose property value will be set.
        //
        //   value:
        //     The property value.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(object instance, object value)
        {
            ValueSetter(instance, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FastProperty GetProperty<T>(Expression<Func<T, object>> property, PropertyCachingStrategy cachingStrategy = PropertyCachingStrategy.Cached)
        {
            return GetProperty(property.ExtractPropertyInfo(), cachingStrategy);
        }

        public static FastProperty GetProperty(Type type, string propertyName, PropertyCachingStrategy cachingStrategy = PropertyCachingStrategy.Cached)
        {
            Guard.NotNull(type, "type");
            Guard.NotEmpty(propertyName, "propertyName");
            if (TryGetCachedProperty(type, propertyName, cachingStrategy == PropertyCachingStrategy.EagerCached, out var fastProperty))
            {
                return fastProperty;
            }

            PropertyKey key = new PropertyKey(type, propertyName);
            if (!_singlePropertiesCache.TryGetValue(key, out fastProperty))
            {
                PropertyInfo property = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (property != null)
                {
                    fastProperty = Create(property);
                    if (cachingStrategy > PropertyCachingStrategy.Uncached)
                    {
                        _singlePropertiesCache.TryAdd(key, fastProperty);
                    }
                }
            }

            return fastProperty;
        }

        public static FastProperty GetProperty(PropertyInfo propertyInfo, PropertyCachingStrategy cachingStrategy = PropertyCachingStrategy.Cached)
        {
            Guard.NotNull(propertyInfo, "propertyInfo");
            if (TryGetCachedProperty(propertyInfo.ReflectedType, propertyInfo.Name, cachingStrategy == PropertyCachingStrategy.EagerCached, out var fastProperty))
            {
                return fastProperty;
            }

            PropertyKey key = new PropertyKey(propertyInfo.ReflectedType, propertyInfo.Name);
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

        private static bool TryGetCachedProperty(Type type, string propertyName, bool eagerCached, out FastProperty fastProperty)
        {
            fastProperty = null;
            IDictionary<string, FastProperty> value;
            if (eagerCached)
            {
                value = (IDictionary<string, FastProperty>)GetProperties(type);
                value.TryGetValue(propertyName, out fastProperty);
            }

            if (fastProperty == null && _propertiesCache.TryGetValue(type, out value))
            {
                value.TryGetValue(propertyName, out fastProperty);
            }

            return fastProperty != null;
        }

        //
        // Summary:
        //     Given an object, adds each instance property with a public get method as a key
        //     and its associated value to a dictionary. If the object is already an IDictionary{string,
        //     object} instance, then a copy is returned.
        //
        // Parameters:
        //   keySelector:
        //     Key selector
        //
        //   deep:
        //     When true, converts all nested objects to dictionaries also
        //
        // Remarks:
        //     The implementation of FastProperty will cache the property accessors per-type.
        //     This is faster when the the same type is used multiple times with ObjectToDictionary.
        public static IDictionary<string, object> ObjectToDictionary(object value, Func<string, string> keySelector = null, bool deep = false)
        {
            IDictionary<string, object> dictionary = value as IDictionary<string, object>;
            if (dictionary != null)
            {
                return new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
            }

            dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            if (value != null)
            {
                keySelector = keySelector ?? ((Func<string, string>)((string key) => key));
                foreach (FastProperty value2 in GetProperties(value.GetType()).Values)
                {
                    object obj = value2.GetValue(value);
                    if (deep && obj != null && value2.Property.PropertyType.IsPlainObjectType())
                    {
                        obj = ObjectToDictionary(obj, null, deep: true);
                    }

                    dictionary[keySelector(value2.Name)] = obj;
                }
            }

            return dictionary;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FastProperty Create(PropertyInfo property)
        {
            return new DelegatedAccessor(property);
        }

        //
        // Summary:
        //     Creates and caches fast property helpers that expose getters for every non-hidden
        //     get property on the specified type.
        //     VTQT.ComponentModel.FastProperty.GetVisibleProperties(System.Type,VTQT.ComponentModel.PropertyCachingStrategy)
        //     excludes properties defined on base types that have been hidden by definitions
        //     using the new keyword.
        //
        // Parameters:
        //   type:
        //     The type to extract property accessors for.
        //
        // Returns:
        //     A cached array of all public property getters from the type.
        public static IReadOnlyDictionary<string, FastProperty> GetVisibleProperties(Type type, PropertyCachingStrategy cachingStrategy = PropertyCachingStrategy.Cached)
        {
            Guard.NotNull(type, "type");
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (_visiblePropertiesCache.TryGetValue(type, out var value))
            {
                return (IReadOnlyDictionary<string, FastProperty>)value;
            }

            ConcurrentDictionary<Type, IDictionary<string, FastProperty>> concurrentDictionary = ((cachingStrategy > PropertyCachingStrategy.Uncached) ? _visiblePropertiesCache : CreateVolatileCache());
            bool flag = true;
            IReadOnlyDictionary<string, FastProperty> properties = GetProperties(type, cachingStrategy);
            foreach (FastProperty value2 in properties.Values)
            {
                if (value2.Property.DeclaringType != type)
                {
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                value = (IDictionary<string, FastProperty>)properties;
                concurrentDictionary.TryAdd(type, value);
                return properties;
            }

            List<FastProperty> list = new List<FastProperty>(properties.Count);
            foreach (FastProperty value3 in properties.Values)
            {
                Type declaringType = value3.Property.DeclaringType;
                if (declaringType == type)
                {
                    list.Add(value3);
                    continue;
                }

                bool flag2 = false;
                TypeInfo typeInfo = type.GetTypeInfo();
                TypeInfo typeInfo2 = declaringType.GetTypeInfo();
                while (typeInfo != null && typeInfo != typeInfo2)
                {
                    PropertyInfo declaredProperty = typeInfo.GetDeclaredProperty(value3.Name);
                    if (declaredProperty != null)
                    {
                        flag2 = true;
                        break;
                    }

                    if (typeInfo.BaseType != null)
                    {
                        typeInfo = typeInfo.BaseType.GetTypeInfo();
                    }
                }

                if (!flag2)
                {
                    list.Add(value3);
                }
            }

            value = list.ToDictionary<FastProperty, string>((FastProperty x) => x.Name, StringComparer.OrdinalIgnoreCase);
            concurrentDictionary.TryAdd(type, value);
            return (IReadOnlyDictionary<string, FastProperty>)value;
        }

        //
        // Summary:
        //     Creates and caches fast property helpers that expose getters for every public
        //     get property on the specified type.
        //
        // Parameters:
        //   type:
        //     The type to extract property accessors for.
        //
        // Returns:
        //     A cached array of all public property getters from the type of target instance.
        public static IReadOnlyDictionary<string, FastProperty> GetProperties(Type type, PropertyCachingStrategy cachingStrategy = PropertyCachingStrategy.Cached)
        {
            Guard.NotNull(type, "type");
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (!_propertiesCache.TryGetValue(type, out var value))
            {
                value = ((cachingStrategy != 0) ? _propertiesCache.GetOrAdd(type, new Func<Type, IDictionary<string, FastProperty>>(Get)) : Get(type));
            }

            return (IReadOnlyDictionary<string, FastProperty>)value;
            static IDictionary<string, FastProperty> Get(Type t)
            {
                IEnumerable<PropertyInfo> candidateProperties = GetCandidateProperties(t);
                Dictionary<string, FastProperty> dictionary = candidateProperties.Select((PropertyInfo p) => Create(p)).ToDictionary<FastProperty, string>((FastProperty x) => x.Name, StringComparer.OrdinalIgnoreCase);
                CleanDuplicates(t, dictionary.Keys);
                return dictionary;
            }
        }

        private static void CleanDuplicates(Type type, IEnumerable<string> propNames)
        {
            foreach (string propName in propNames)
            {
                PropertyKey key = new PropertyKey(type, propName);
                _singlePropertiesCache.TryRemove(key, out var _);
            }
        }

        internal static IEnumerable<PropertyInfo> GetCandidateProperties(Type type)
        {
            IEnumerable<PropertyInfo> enumerable = type.GetRuntimeProperties().Where(IsCandidateProperty);
            TypeInfo typeInfo = type.GetTypeInfo();
            if (typeInfo.IsInterface)
            {
                enumerable = enumerable.Concat<PropertyInfo>(typeInfo.ImplementedInterfaces.SelectMany((Type interfaceType) => interfaceType.GetRuntimeProperties().Where(IsCandidateProperty)));
            }

            return enumerable;
        }

        private static bool IsCandidateProperty(PropertyInfo property)
        {
            return property.GetIndexParameters().Length == 0 && property.GetMethod != null && property.GetMethod!.IsPublic && !property.GetMethod!.IsStatic;
        }

        private static ConcurrentDictionary<Type, IDictionary<string, FastProperty>> CreateVolatileCache()
        {
            return new ConcurrentDictionary<Type, IDictionary<string, FastProperty>>();
        }
    }



    [DebuggerDisplay("DelegateAccessor: {Name}")]
    internal sealed class DelegatedAccessor : FastProperty
    {
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
            return MakeFastPropertyGetter(propertyInfo, CallPropertyGetterOpenGenericMethod, CallPropertyGetterByReferenceOpenGenericMethod);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Action<object, object> MakePropertySetter(PropertyInfo propertyInfo)
        {
            Debug.Assert(propertyInfo != null);
            Debug.Assert(!propertyInfo.DeclaringType.GetTypeInfo().IsValueType);
            MethodInfo setMethod = propertyInfo.SetMethod;
            Debug.Assert(setMethod != null);
            Debug.Assert(!setMethod.IsStatic);
            Debug.Assert(setMethod.ReturnType == typeof(void));
            ParameterInfo[] parameters = setMethod.GetParameters();
            Debug.Assert(parameters.Length == 1);
            Type declaringType = setMethod.DeclaringType;
            Type parameterType = parameters[0].ParameterType;
            Delegate target = setMethod.CreateDelegate(typeof(Action<,>).MakeGenericType(declaringType, parameterType));
            MethodInfo methodInfo = CallPropertySetterOpenGenericMethod.MakeGenericMethod(declaringType, parameterType);
            Delegate @delegate = methodInfo.CreateDelegate(typeof(Action<object, object>), target);
            return (Action<object, object>)@delegate;
        }

        //
        // Summary:
        //     Creates a single fast property getter which is safe for a null input object.
        //     The result is not cached.
        //
        // Parameters:
        //   propertyInfo:
        //     propertyInfo to extract the getter for.
        //
        // Returns:
        //     A fast getter.
        //
        // Remarks:
        //     This method is more memory efficient than a dynamically compiled lambda, and
        //     about the same speed.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Func<object, object> MakeNullSafeFastPropertyGetter(PropertyInfo propertyInfo)
        {
            Debug.Assert(propertyInfo != null);
            return MakeFastPropertyGetter(propertyInfo, CallNullSafePropertyGetterOpenGenericMethod, CallNullSafePropertyGetterByReferenceOpenGenericMethod);
        }

        private static Func<object, object> MakeFastPropertyGetter(PropertyInfo propertyInfo, MethodInfo propertyGetterWrapperMethod, MethodInfo propertyGetterByRefWrapperMethod)
        {
            Debug.Assert(propertyInfo != null);
            Debug.Assert(propertyGetterWrapperMethod != null);
            Debug.Assert(propertyGetterWrapperMethod.IsGenericMethodDefinition);
            Debug.Assert(propertyGetterWrapperMethod.GetParameters().Length == 2);
            Debug.Assert(propertyGetterByRefWrapperMethod != null);
            Debug.Assert(propertyGetterByRefWrapperMethod.IsGenericMethodDefinition);
            Debug.Assert(propertyGetterByRefWrapperMethod.GetParameters().Length == 2);
            MethodInfo getMethod = propertyInfo.GetMethod;
            Debug.Assert(getMethod != null);
            Debug.Assert(!getMethod.IsStatic);
            Debug.Assert(getMethod.GetParameters().Length == 0);
            if (getMethod.DeclaringType.GetTypeInfo().IsValueType)
            {
                return MakeFastPropertyGetter(typeof(ByRefFunc<,>), getMethod, propertyGetterByRefWrapperMethod);
            }

            return MakeFastPropertyGetter(typeof(Func<,>), getMethod, propertyGetterWrapperMethod);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Func<object, object> MakeFastPropertyGetter(Type openGenericDelegateType, MethodInfo propertyGetMethod, MethodInfo openGenericWrapperMethod)
        {
            Type declaringType = propertyGetMethod.DeclaringType;
            Type returnType = propertyGetMethod.ReturnType;
            Type delegateType = openGenericDelegateType.MakeGenericType(declaringType, returnType);
            Delegate target = propertyGetMethod.CreateDelegate(delegateType);
            MethodInfo methodInfo = openGenericWrapperMethod.MakeGenericMethod(declaringType, returnType);
            Delegate @delegate = methodInfo.CreateDelegate(typeof(Func<object, object>), target);
            return (Func<object, object>)@delegate;
        }

        private static object CallPropertyGetter<TDeclaringType, TValue>(Func<TDeclaringType, TValue> getter, object target)
        {
            return getter((TDeclaringType)target);
        }

        private static object CallPropertyGetterByReference<TDeclaringType, TValue>(ByRefFunc<TDeclaringType, TValue> getter, object target)
        {
            TDeclaringType arg = (TDeclaringType)target;
            return getter(ref arg);
        }

        private static object CallNullSafePropertyGetter<TDeclaringType, TValue>(Func<TDeclaringType, TValue> getter, object target)
        {
            if (target == null)
            {
                return null;
            }

            return getter((TDeclaringType)target);
        }

        private static object CallNullSafePropertyGetterByReference<TDeclaringType, TValue>(ByRefFunc<TDeclaringType, TValue> getter, object target)
        {
            if (target == null)
            {
                return null;
            }

            TDeclaringType arg = (TDeclaringType)target;
            return getter(ref arg);
        }

        private static void CallPropertySetter<TDeclaringType, TValue>(Action<TDeclaringType, TValue> setter, object target, object value)
        {
            setter((TDeclaringType)target, (TValue)value);
        }
    }
}
