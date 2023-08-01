using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Collections;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Share.BaseCore.Extensions
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
            where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key)
            where T : class
        {
            tempData.TryGetValue(key, out object o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }

        public static T Peek<T>(this ITempDataDictionary tempData, string key)
            where T : class
        {
            object o = tempData.Peek(key);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }
    public static class GenericTypeExtensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = string.Empty;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();
        }
    }

    public static class CollectionSlicer
    {
        /// <summary>
        /// Slices the iteration over an enumerable by the given slice sizes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source sequence to slice</param>
        /// <param name="sizes">
        /// Slice sizes. At least one size is required. Multiple sizes result in differently sized slices,
        /// whereat the last size is used for the "rest" (if any)
        /// </param>
        /// <returns>The sliced enumerable</returns>
        public static IEnumerable<IEnumerable<T>> Slice<T>(this IEnumerable<T> source, params int[] sizes)
        {
            if (!sizes.Any(step => step != 0))
            {
                throw new InvalidOperationException("Can't slice a collection with step length 0.");
            }

            return new Slicer<T>(source.GetEnumerator(), sizes).Slice();
        }
    }

    internal sealed class Slicer<T>
    {
        private readonly IEnumerator<T> _iterator;
        private readonly int[] _sizes;
        private volatile bool _hasNext;
        private volatile int _currentSize;
        private volatile int _index;

        public Slicer(IEnumerator<T> iterator, int[] sizes)
        {
            _iterator = iterator;
            _sizes = sizes;
            _index = 0;
            _currentSize = 0;
            _hasNext = true;
        }

        public int Index
        {
            get { return _index; }
        }

        public IEnumerable<IEnumerable<T>> Slice()
        {
            var length = _sizes.Length;
            var index = 1;
            var size = 0;

            for (var i = 0; _hasNext; ++i)
            {
                if (i < length)
                {
                    size = _sizes[i];
                    _currentSize = size - 1;
                }

                while (_index < index && _hasNext)
                {
                    _hasNext = MoveNext();
                }

                if (_hasNext)
                {
                    yield return new List<T>(SliceInternal());
                    index += size;
                }
            }
        }

        private IEnumerable<T> SliceInternal()
        {
            if (_currentSize == -1) yield break;
            yield return _iterator.Current;

            for (var count = 0; count < _currentSize && _hasNext; ++count)
            {
                _hasNext = MoveNext();

                if (_hasNext)
                {
                    yield return _iterator.Current;
                }
            }
        }

        private bool MoveNext()
        {
            ++_index;
            return _iterator.MoveNext();
        }
    }

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static class EnumerableExtensions
    {
        #region Nested classes

        private static class DefaultReadOnlyCollection<T>
        {
            private static ReadOnlyCollection<T> defaultCollection;

            [SuppressMessage("ReSharper", "ConvertIfStatementToNullCoalescingExpression")]
            internal static ReadOnlyCollection<T> Empty
            {
                get
                {
                    if (defaultCollection == null)
                    {
                        defaultCollection = new ReadOnlyCollection<T>(new T[0]);
                    }
                    return defaultCollection;
                }
            }
        }

        #endregion

        #region IEnumerable

        /// <summary>
        /// Performs an action on each item while iterating through a list. 
        /// This is a handy shortcut for <c>foreach(item in list) { ... }</c>
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="source">The list, which holds the objects.</param>
        /// <param name="action">The action delegate which is called on each item while iterating.</param>
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T t in source)
            {
                action(t);
            }
        }

        /// <summary>
        /// Performs an action on each item while iterating through a list. 
        /// This is a handy shortcut for <c>foreach(item in list) { ... }</c>
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="source">The list, which holds the objects.</param>
        /// <param name="action">The action delegate which is called on each item while iterating.</param>
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int i = 0;
            foreach (T t in source)
            {
                action(t, i++);
            }
        }

        public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> source)
        {
            if (source == null || !source.Any())
                return DefaultReadOnlyCollection<T>.Empty;

            if (source is ReadOnlyCollection<T> readOnly)
            {
                return readOnly;
            }
            else if (source is List<T> list)
            {
                return list.AsReadOnly();
            }

            return new ReadOnlyCollection<T>(source.ToList());
        }

        /// <summary>
        /// Converts an enumerable to a dictionary while tolerating duplicate entries (last wins)
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="keySelector">keySelector</param>
        /// <returns>Result as dictionary</returns>
        public static Dictionary<TKey, TSource> ToDictionarySafe<TSource, TKey>(
            this IEnumerable<TSource> source,
             Func<TSource, TKey> keySelector)
        {
            return source.ToDictionarySafe(keySelector, new Func<TSource, TSource>(src => src), null);
        }

        /// <summary>
        /// Converts an enumerable to a dictionary while tolerating duplicate entries (last wins)
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="keySelector">keySelector</param>
        /// <param name="comparer">comparer</param>
        /// <returns>Result as dictionary</returns>
        public static Dictionary<TKey, TSource> ToDictionarySafe<TSource, TKey>(
            this IEnumerable<TSource> source,
             Func<TSource, TKey> keySelector,
             IEqualityComparer<TKey> comparer)
        {
            return source.ToDictionarySafe(keySelector, new Func<TSource, TSource>(src => src), comparer);
        }

        /// <summary>
        /// Converts an enumerable to a dictionary while tolerating duplicate entries (last wins)
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="keySelector">keySelector</param>
        /// <param name="elementSelector">elementSelector</param>
        /// <returns>Result as dictionary</returns>
        public static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
             Func<TSource, TKey> keySelector,
             Func<TSource, TElement> elementSelector)
        {
            return source.ToDictionarySafe(keySelector, elementSelector, null);
        }

        /// <summary>
        /// Converts an enumerable to a dictionary while tolerating duplicate entries (last wins)
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="keySelector">keySelector</param>
        /// <param name="elementSelector">elementSelector</param>
        /// <param name="comparer">comparer</param>
        /// <returns>Result as dictionary</returns>
        public static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
             Func<TSource, TKey> keySelector,
             Func<TSource, TElement> elementSelector,
             IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            var dictionary = new Dictionary<TKey, TElement>(comparer);

            foreach (var local in source)
            {
                dictionary[keySelector(local)] = elementSelector(local);
            }

            return dictionary;
        }

        /// <summary>The distinct by.</summary>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <returns>the unique list</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            where TKey : IEquatable<TKey>
        {
            return source.Distinct(GenericEqualityComparer<TSource>.CompareMember(keySelector));
        }

        /// <summary>
        /// Orders a collection of entities by a specific ID sequence
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="source">The entity collection to sort</param>
        /// <param name="ids">The IDs to order by</param>
        /// <returns>The sorted entity collection</returns>
        public static IEnumerable<TEntity> OrderBySequence<TEntity>(this IEnumerable<TEntity> source, IEnumerable<string> ids) where TEntity : BaseEntity
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var sorted = from id in ids
                         join entity in source on id equals entity.Id
                         select entity;

            return sorted;
        }

        public static string StrJoin(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }

        #endregion
        /// <summary>
        /// A data structure that contains multiple values for each key.
        /// </summary>
        /// <typeparam name="TKey">The type of key.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        [JsonConverter(typeof(MultiMapConverter))]
        [Serializable]
        public class Multimap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, ICollection<TValue>>>
        {
            private readonly IDictionary<TKey, ICollection<TValue>> _dict;
            private readonly Func<IEnumerable<TValue>, ICollection<TValue>> _collectionCreator;
            private readonly bool _isReadonly = false;

            internal readonly static Func<IEnumerable<TValue>, ICollection<TValue>> DefaultCollectionCreator =
                x => new List<TValue>(x ?? Enumerable.Empty<TValue>());

            public Multimap()
                : this(EqualityComparer<TKey>.Default)
            {
            }

            public Multimap(IEqualityComparer<TKey> comparer)
            {
                _dict = new Dictionary<TKey, ICollection<TValue>>(comparer ?? EqualityComparer<TKey>.Default);
                _collectionCreator = DefaultCollectionCreator;
            }

            public Multimap(Func<IEnumerable<TValue>, ICollection<TValue>> collectionCreator)
                : this(new Dictionary<TKey, ICollection<TValue>>(), collectionCreator)
            {
            }

            public Multimap(IEqualityComparer<TKey> comparer, Func<IEnumerable<TValue>, ICollection<TValue>> collectionCreator)
                : this(new Dictionary<TKey, ICollection<TValue>>(comparer ?? EqualityComparer<TKey>.Default), collectionCreator)
            {
            }

            internal Multimap(IDictionary<TKey, ICollection<TValue>> dictionary, Func<IEnumerable<TValue>, ICollection<TValue>> collectionCreator)
            {
                Guard.NotNull(dictionary, nameof(dictionary));
                Guard.NotNull(collectionCreator, nameof(collectionCreator));

                _dict = dictionary;
                _collectionCreator = collectionCreator;
            }

            protected Multimap(IDictionary<TKey, ICollection<TValue>> dictionary, bool isReadonly)
            {
                Guard.NotNull(dictionary, nameof(dictionary));

                _dict = dictionary;

                if (isReadonly && dictionary != null)
                {
                    foreach (var kvp in dictionary)
                    {
                        dictionary[kvp.Key] = kvp.Value.AsReadOnly();
                    }
                }

                _isReadonly = isReadonly;
            }

            public Multimap(IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> items)
                : this(items, null)
            {
                // for serialization
            }

            public Multimap(IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> items, IEqualityComparer<TKey> comparer)
            {
                // for serialization
                Guard.NotNull(items, nameof(items));

                _dict = new Dictionary<TKey, ICollection<TValue>>(comparer ?? EqualityComparer<TKey>.Default);

                if (items != null)
                {
                    foreach (var kvp in items)
                    {
                        _dict[kvp.Key] = CreateCollection(kvp.Value);
                    }
                }
            }

            protected virtual ICollection<TValue> CreateCollection(IEnumerable<TValue> values)
            {
                return (_collectionCreator ?? DefaultCollectionCreator)(values ?? Enumerable.Empty<TValue>());
            }

            /// <summary>
            /// Gets the count of groups/keys.
            /// </summary>
            public int Count
            {
                get
                {
                    return this._dict.Keys.Count;
                }
            }

            /// <summary>
            /// Gets the total count of items in all groups.
            /// </summary>
            public int TotalValueCount
            {
                get
                {
                    return this._dict.Values.Sum(x => x.Count);
                }
            }

            /// <summary>
            /// Gets the collection of values stored under the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            public virtual ICollection<TValue> this[TKey key]
            {
                get
                {
                    if (!_dict.ContainsKey(key))
                    {
                        if (!_isReadonly)
                            _dict[key] = CreateCollection(null);
                        else
                            return null;
                    }

                    return _dict[key];
                }
            }

            /// <summary>
            /// Gets the collection of keys.
            /// </summary>
            public virtual ICollection<TKey> Keys
            {
                get { return _dict.Keys; }
            }

            /// <summary>
            /// Gets all value collections.
            /// </summary>
            public virtual ICollection<ICollection<TValue>> Values
            {
                get { return _dict.Values; }
            }

            public IEnumerable<TValue> Find(TKey key, Expression<Func<TValue, bool>> predicate)
            {
                Guard.NotNull(key, nameof(key));
                Guard.NotNull(predicate, nameof(predicate));

                if (_dict.ContainsKey(key))
                {
                    return _dict[key].Where(predicate.Compile());
                }

                return Enumerable.Empty<TValue>();
            }

            /// <summary>
            /// Adds the specified value for the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            public virtual void Add(TKey key, TValue value)
            {
                CheckNotReadonly();

                this[key].Add(value);
            }

            /// <summary>
            /// Adds the specified values to the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="values">The values.</param>
            public virtual void AddRange(TKey key, IEnumerable<TValue> values)
            {
                if (values == null || !values.Any())
                    return;

                CheckNotReadonly();

                this[key].AddRange(values);
            }

            /// <summary>
            /// Removes the specified value for the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            /// <returns><c>True</c> if such a value existed and was removed; otherwise <c>false</c>.</returns>
            public virtual bool Remove(TKey key, TValue value)
            {
                CheckNotReadonly();

                if (!_dict.ContainsKey(key))
                    return false;

                bool result = _dict[key].Remove(value);
                if (_dict[key].Count == 0)
                    _dict.Remove(key);

                return result;
            }

            /// <summary>
            /// Removes all values for the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns><c>True</c> if any such values existed; otherwise <c>false</c>.</returns>
            public virtual bool RemoveAll(TKey key)
            {
                CheckNotReadonly();
                return _dict.Remove(key);
            }

            /// <summary>
            /// Removes all values.
            /// </summary>
            public virtual void Clear()
            {
                CheckNotReadonly();
                _dict.Clear();
            }

            /// <summary>
            /// Determines whether the multimap contains any values for the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns><c>True</c> if the multimap has one or more values for the specified key, otherwise <c>false</c>.</returns>
            public virtual bool ContainsKey(TKey key)
            {
                return _dict.ContainsKey(key);
            }

            /// <summary>
            /// Determines whether the multimap contains the specified value for the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            /// <returns><c>True</c> if the multimap contains such a value; otherwise, <c>false</c>.</returns>
            public virtual bool ContainsValue(TKey key, TValue value)
            {
                return _dict.ContainsKey(key) && _dict[key].Contains(value);
            }

            /// <summary>
            /// Returns an enumerator that iterates through the multimap.
            /// </summary>
            /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the multimap.</returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <summary>
            /// Returns an enumerator that iterates through the multimap.
            /// </summary>
            /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the multimap.</returns>
            public virtual IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator()
            {
                foreach (KeyValuePair<TKey, ICollection<TValue>> pair in _dict)
                    yield return pair;
            }

            private void CheckNotReadonly()
            {
                if (_isReadonly)
                    throw new NotSupportedException("Multimap is read-only.");
            }

            #region Static members

            public static Multimap<TKey, TValue> CreateFromLookup(ILookup<TKey, TValue> source)
            {
                Guard.NotNull(source, nameof(source));

                var map = new Multimap<TKey, TValue>();

                foreach (IGrouping<TKey, TValue> group in source)
                {
                    map.AddRange(group.Key, group);
                }

                return map;
            }

            #endregion
        }

        public class MultiMapConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                var canConvert = objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Multimap<,>);
                return canConvert;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                // typeof TKey
                var keyType = objectType.GetGenericArguments()[0];

                // typeof TValue
                var valueType = objectType.GetGenericArguments()[1];

                // typeof IEnumerable<KeyValuePair<TKey, ICollection<TValue>>
                var sequenceType = typeof(IEnumerable<>).MakeGenericType(typeof(KeyValuePair<,>).MakeGenericType(keyType, typeof(IEnumerable<>).MakeGenericType(valueType)));

                // serialize JArray to sequenceType
                var list = serializer.Deserialize(reader, sequenceType);

                if (keyType == typeof(string))
                {
                    // call constructor Multimap(IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> items, IEqualityComparer<TKey> comparer)
                    // TBD: we always assume string keys to be case insensitive. Serialize it somehow and fetch here!
                    return Activator.CreateInstance(objectType, new object[] { list, StringComparer.OrdinalIgnoreCase });
                }
                else
                {
                    // call constructor Multimap(IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> items)
                    return Activator.CreateInstance(objectType, new object[] { list });
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                {
                    var enumerable = value as IEnumerable;
                    foreach (var item in enumerable)
                    {
                        // Json.Net uses a converter for KeyValuePair here
                        serializer.Serialize(writer, item);
                    }
                }
                writer.WriteEndArray();
            }
        }
        #region Multimap

        public static Multimap<TKey, TValue> ToMultimap<TSource, TKey, TValue>(
                                                this IEnumerable<TSource> source,
                                                Func<TSource, TKey> keySelector,
                                                Func<TSource, TValue> valueSelector)
        {
            return source.ToMultimap(keySelector, valueSelector, null);
        }

        public static Multimap<TKey, TValue> ToMultimap<TSource, TKey, TValue>(
                                                this IEnumerable<TSource> source,
                                                Func<TSource, TKey> keySelector,
                                                Func<TSource, TValue> valueSelector,
                                                IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            var map = new Multimap<TKey, TValue>(comparer);

            foreach (var item in source)
            {
                map.Add(keySelector(item), valueSelector(item));
            }

            return map;
        }

        #endregion

        #region NameValueCollection

        public static void AddRange(this NameValueCollection initial, NameValueCollection other)
        {
            if (initial == null)
                throw new ArgumentNullException(nameof(initial));

            if (other == null)
                return;

            foreach (var item in other.AllKeys)
            {
                initial.Add(item, other[item]);
            }
        }

        /// <summary>
        /// Builds an URL query string
        /// </summary>
        /// <param name="nvc">Name value collection</param>
        /// <param name="encoding">Encoding type. Can be null.</param>
        /// <param name="encode">Whether to encode keys and values</param>
        /// <returns>The query string without leading a question mark</returns>
        public static string BuildQueryString(this NameValueCollection nvc, Encoding encoding, bool encode = true)
        {
            var sb = new StringBuilder();

            if (nvc != null)
            {
                foreach (string str in nvc)
                {
                    if (sb.Length > 0)
                        sb.Append('&');

                    if (!encode)
                        sb.Append(str);
                    else if (encoding == null)
                        sb.Append(HttpUtility.UrlEncode(str));
                    else
                        sb.Append(HttpUtility.UrlEncode(str, encoding));

                    sb.Append('=');

                    if (!encode)
                        sb.Append(nvc[str]);
                    else if (encoding == null)
                        sb.Append(HttpUtility.UrlEncode(nvc[str]));
                    else
                        sb.Append(HttpUtility.UrlEncode(nvc[str], encoding));
                }
            }

            return sb.ToString();
        }

        #endregion

        #region List

        /// <summary>
        /// Safe way to remove selected entries from a list.
        /// </summary>
        /// <remarks>To be used for materialized lists only, not IEnumerable or similar.</remarks>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="list">List.</param>
        /// <param name="selector">Selector for the entries to be removed.</param>
        /// <returns>Number of removed entries.</returns>
        public static int Remove<T>(this IList<T> list, Func<T, bool> selector)
        {
            Guard.NotNull(list, nameof(list));
            Guard.NotNull(selector, nameof(selector));

            var count = 0;
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (selector(list[i]))
                {
                    list.RemoveAt(i);
                    ++count;
                }
            }

            return count;
        }

        #endregion

        #region Stack

        public static bool TryPeek<T>(this Stack<T> stack, out T value)
        {
            value = default(T);

            if (stack.Count > 0)
            {
                value = stack.Peek();
                return true;
            }

            return false;
        }

        public static bool TryPop<T>(this Stack<T> stack, out T value)
        {
            value = default(T);

            if (stack.Count > 0)
            {
                value = stack.Pop();
                return true;
            }

            return false;
        }

        #endregion
    }
    public sealed partial class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equals;
        private readonly Func<T, int> _getHashcode;

        public GenericEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashcode)
        {
            _getHashcode = getHashcode;
            _equals = equals;
        }

        public static GenericEqualityComparer<T> CompareMember<TMember>(Func<T, TMember> memberExpression) where TMember : IEquatable<TMember>
        {
            return new GenericEqualityComparer<T>(
                (x, y) => memberExpression.Invoke(x).Equals((TMember)memberExpression.Invoke(y)),
                x =>
                {
                    var invoked = memberExpression.Invoke(x);
                    return !ReferenceEquals(invoked, default(TMember)) ? invoked.GetHashCode() : 0;
                });
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
        public bool Equals(T x, T y)
        {
            return _equals.Invoke(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(T obj)
        {
            return _getHashcode.Invoke(obj);
        }
    }

}
