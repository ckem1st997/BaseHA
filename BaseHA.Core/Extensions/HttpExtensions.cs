using BaseHA.Core.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseHA.Core.Extensions
{
    public static class HttpExtensions
    {
        private const string CacheRegionName = "Base:";

        private const string RememberPathKey = "AppRelativeCurrentExecutionFilePath.Original";

        private static readonly List<Tuple<string, string>> _sslHeaders = new List<Tuple<string, string>>
        {
            new Tuple<string, string>("HTTP_CLUSTER_HTTPS", "on"),
            new Tuple<string, string>("HTTP_X_FORWARDED_PROTO", "https"),
            new Tuple<string, string>("X-Forwarded-Proto", "https"),
            new Tuple<string, string>("x-arr-ssl", null),
            new Tuple<string, string>("X-Forwarded-Protocol", "https"),
            new Tuple<string, string>("X-Forwarded-Ssl", "on"),
            new Tuple<string, string>("X-Url-Scheme", "https")
        };


        //
        // Summary:
        //     Gets a value which indicates whether the HTTP connection uses secure sockets
        //     (HTTPS protocol). Works with Cloud's load balancers.
        public static bool IsHttps(this HttpRequest request)
        {
            if (request.IsHttps)
            {
                return true;
            }

            foreach (Tuple<string, string> sslHeader in _sslHeaders)
            {
                string serverVariable = request.HttpContext.GetServerVariable(sslHeader.Item1);
                if (serverVariable != null)
                {
                    return sslHeader.Item2 == null || sslHeader.Item2.Equals(serverVariable, StringComparison.OrdinalIgnoreCase);
                }
            }

            return false;
        }

        public static T GetItem<T>(this HttpContext httpContext, string key, Func<T> factory = null, bool forceCreation = true)
        {
            Guard.NotEmpty(key, "key");
            IDictionary<object, object> dictionary = httpContext?.Items;
            if (dictionary == null)
            {
                return default(T);
            }

            if (dictionary.ContainsKey(key))
            {
                return (T)dictionary[key];
            }

            if (forceCreation)
            {
                object obj2 = (dictionary[key] = (factory ?? ((Func<T>)(() => Activator.CreateInstance<T>())))());
                object obj3 = obj2;
                return (T)obj3;
            }

            return default(T);
        }

        //
        // Summary:
        //     Determines whether the specified HTTP request is an AJAX request.
        //
        // Parameters:
        //   request:
        //     The HTTP request.
        //
        // Returns:
        //     true if the specified HTTP request is an AJAX request; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The request parameter is null (Nothing in Visual Basic).
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return request.Headers != null && request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public static string GetValue(this HttpRequest request, string key)
        {
            string text = string.Empty;
            if (request.HasFormContentType && request.Form != null && request.Form.Any())
            {
                text = request.Form[key];
            }

            if (text.IsEmpty())
            {
                text = request.Query[key];
            }

            if (text.IsEmpty())
            {
                text = request.Cookies[key];
            }

            if (text.IsEmpty())
            {
                text = request.HttpContext.GetServerVariable(key);
            }

            return text ?? string.Empty;
        }
    }

}
