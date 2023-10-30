using BaseHA.Core.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BaseHA.Core.Extensions
{
    public static class EnumerableExtensions
    {
        private static class DefaultReadOnlyCollection<T>
        {
            private static ReadOnlyCollection<T> defaultCollection;

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

        //
        // Summary:
        //     Performs an action on each item while iterating through a list. This is a handy
        //     shortcut for foreach(item in list) { ... }
        //
        // Parameters:
        //   source:
        //     The list, which holds the objects.
        //
        //   action:
        //     The action delegate which is called on each item while iterating.
        //
        // Type parameters:
        //   T:
        //     The type of the items.
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        //
        // Summary:
        //     Performs an action on each item while iterating through a list. This is a handy
        //     shortcut for foreach(item in list) { ... }
        //
        // Parameters:
        //   source:
        //     The list, which holds the objects.
        //
        //   action:
        //     The action delegate which is called on each item while iterating.
        //
        // Type parameters:
        //   T:
        //     The type of the items.
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int num = 0;
            foreach (T item in source)
            {
                action(item, num++);
            }
        }

        public static ReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> source)
        {
            if (source == null || !source.Any())
            {
                return DefaultReadOnlyCollection<T>.Empty;
            }

            ReadOnlyCollection<T> readOnlyCollection = source as ReadOnlyCollection<T>;
            if (readOnlyCollection != null)
            {
                return readOnlyCollection;
            }

            List<T> list = source as List<T>;
            if (list != null)
            {
                return list.AsReadOnly();
            }

            return new ReadOnlyCollection<T>(source.ToList());
        }

        //
        // Summary:
        //     Converts an enumerable to a dictionary while tolerating duplicate entries (last
        //     wins)
        //
        // Parameters:
        //   source:
        //     source
        //
        //   keySelector:
        //     keySelector
        //
        // Returns:
        //     Result as dictionary
        public static Dictionary<TKey, TSource> ToDictionarySafe<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.ToDictionarySafe(keySelector, (TSource src) => src, null);
        }

        //
        // Summary:
        //     Converts an enumerable to a dictionary while tolerating duplicate entries (last
        //     wins)
        //
        // Parameters:
        //   source:
        //     source
        //
        //   keySelector:
        //     keySelector
        //
        //   comparer:
        //     comparer
        //
        // Returns:
        //     Result as dictionary
        public static Dictionary<TKey, TSource> ToDictionarySafe<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return source.ToDictionarySafe(keySelector, (TSource src) => src, comparer);
        }

        //
        // Summary:
        //     Converts an enumerable to a dictionary while tolerating duplicate entries (last
        //     wins)
        //
        // Parameters:
        //   source:
        //     source
        //
        //   keySelector:
        //     keySelector
        //
        //   elementSelector:
        //     elementSelector
        //
        // Returns:
        //     Result as dictionary
        public static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return source.ToDictionarySafe(keySelector, elementSelector, null);
        }

        //
        // Summary:
        //     Converts an enumerable to a dictionary while tolerating duplicate entries (last
        //     wins)
        //
        // Parameters:
        //   source:
        //     source
        //
        //   keySelector:
        //     keySelector
        //
        //   elementSelector:
        //     elementSelector
        //
        //   comparer:
        //     comparer
        //
        // Returns:
        //     Result as dictionary
        public static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            if (elementSelector == null)
            {
                throw new ArgumentNullException("elementSelector");
            }

            Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>(comparer);
            foreach (TSource item in source)
            {
                dictionary[keySelector(item)] = elementSelector(item);
            }

            return dictionary;
        }

    

        //
        // Summary:
        //     Orders a collection of entities by a specific ID sequence
        //
        // Parameters:
        //   source:
        //     The entity collection to sort
        //
        //   ids:
        //     The IDs to order by
        //
        // Type parameters:
        //   TEntity:
        //     Entity type
        //
        // Returns:
        //     The sorted entity collection
        public static IEnumerable<TEntity> OrderBySequence<TEntity>(this IEnumerable<TEntity> source, IEnumerable<string> ids) where TEntity : BaseEntity
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (ids == null)
            {
                throw new ArgumentNullException("ids");
            }

            return from id in ids
                   join entity in source on id equals entity.Id
                   select entity;
        }

        public static string StrJoin(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }
        public static void AddRange(this NameValueCollection initial, NameValueCollection other)
        {
            if (initial == null)
            {
                throw new ArgumentNullException("initial");
            }

            if (other != null)
            {
                string[] allKeys = other.AllKeys;
                foreach (string name in allKeys)
                {
                    initial.Add(name, other[name]);
                }
            }
        }

        //
        // Summary:
        //     Builds an URL query string
        //
        // Parameters:
        //   nvc:
        //     Name value collection
        //
        //   encoding:
        //     Encoding type. Can be null.
        //
        //   encode:
        //     Whether to encode keys and values
        //
        // Returns:
        //     The query string without leading a question mark
        public static string BuildQueryString(this NameValueCollection nvc, Encoding encoding, bool encode = true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (nvc != null)
            {
                foreach (string item in nvc)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append('&');
                    }

                    if (!encode)
                    {
                        stringBuilder.Append(item);
                    }
                    else if (encoding == null)
                    {
                        stringBuilder.Append(HttpUtility.UrlEncode(item));
                    }
                    else
                    {
                        stringBuilder.Append(HttpUtility.UrlEncode(item, encoding));
                    }

                    stringBuilder.Append('=');
                    if (!encode)
                    {
                        stringBuilder.Append(nvc[item]);
                    }
                    else if (encoding == null)
                    {
                        stringBuilder.Append(HttpUtility.UrlEncode(nvc[item]));
                    }
                    else
                    {
                        stringBuilder.Append(HttpUtility.UrlEncode(nvc[item], encoding));
                    }
                }
            }

            return stringBuilder.ToString();
        }

        //
        // Summary:
        //     Safe way to remove selected entries from a list.
        //
        // Parameters:
        //   list:
        //     List.
        //
        //   selector:
        //     Selector for the entries to be removed.
        //
        // Type parameters:
        //   T:
        //     Object type.
        //
        // Returns:
        //     Number of removed entries.
        //
        // Remarks:
        //     To be used for materialized lists only, not IEnumerable or similar.
        public static int Remove<T>(this IList<T> list, Func<T, bool> selector)
        {
            Guard.NotNull(list, "list");
            Guard.NotNull(selector, "selector");
            int num = 0;
            for (int num2 = list.Count - 1; num2 >= 0; num2--)
            {
                if (selector(list[num2]))
                {
                    list.RemoveAt(num2);
                    num++;
                }
            }

            return num;
        }

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
    }

}
