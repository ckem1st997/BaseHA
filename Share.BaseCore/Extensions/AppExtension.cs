using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.BaseCore.Extensions
{
    public static class AppExtension
    {
        public static bool IsNullOrEmpty(this string v)
        {
            return string.IsNullOrEmpty(v);
        }

        public static void Remove<TSource>(this IList<TSource> source, Func<TSource, bool> func)
        {
            if (source.AnyList())
            {
                var clear = source.Where(func).ToList();
                foreach (var item in clear)
                {
                    source.Remove(item);
                }
            }
        }
        public static bool AnyList<TSource>(this IEnumerable<TSource> source)
        {
            return source != null && source.Any();
        }

        public static bool NumberZero(this int source)
        {
            return source < 1;
        }
        public static int NumberZero(this int source, int s)
        {
            return source < 1 ? s : source;
        }
        public static bool CheckTryParseNumber(string id)
        {

            bool isNumeric = int.TryParse(id, out int number);
            return isNumeric;
        }
    }
}
