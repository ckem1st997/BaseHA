using BaseHA.Core.Base;
using BaseHA.Core.IRepositories;
using BaseHA.Core.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace BaseHA.Core.Extensions
{
    public static class IMvcNotifierExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Information(this INotifier notifier, string message, bool durable = true)
        {
            notifier.Add(NotifyType.Info, message, durable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Success(this INotifier notifier, string message, bool durable = true)
        {
            notifier.Add(NotifyType.Success, message, durable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Warning(this INotifier notifier, string message, bool durable = true)
        {
            notifier.Add(NotifyType.Warning, message, durable);
        }

        public static void Error(this INotifier notifier, Exception exception, bool durable = true)
        {
            if (exception != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }

                notifier.Add(NotifyType.Error, exception.Message, durable);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Error(this INotifier notifier, string message, bool durable = true)
        {
            notifier.Add(NotifyType.Error, message, durable);
        }
    }

    public static class RouteExtensions
    {
        public static string GetAreaName(this RouteData routeData)
        {
            if (routeData.DataTokens.TryGetValue("area", out var value))
            {
                return value as string;
            }
            return null;
        }

        public static string GetAreaName(this RouteBase route)
        {
            return route?.DataTokens?["area"] as string;
        }

        /// <summary>
        /// Generates an identifier for the given route in the form "[{area}.]{controller}.{action}"
        /// </summary>
        public static string GenerateRouteIdentifier(this RouteData routeData)
        {
            string area = routeData.GetAreaName();
            string controller = routeData.Values["controller"].ToString();
            string action = routeData.Values["action"].ToString();

            return "{0}{1}.{2}".FormatInvariant(
                area.HasValue() ? area + "." : "",
                controller,
                action);
        }

        public static bool IsRouteEqual(this RouteData routeData, string controller, string action)
        {
            if (routeData == null)
                return false;

            return routeData.Values["controller"].ToString().IsCaseInsensitiveEqual(controller)
                   && routeData.Values["action"].ToString().IsCaseInsensitiveEqual(action);
        }

        #region Custom: XBase

        public static string GetControllerName(this RouteData routeData)
        {
            return routeData.Values["controller"].ToString();
        }

        public static string GetActionName(this RouteData routeData)
        {
            return routeData.Values["action"].ToString();
        }


        #endregion

    }

    public static class ExtensionFull
    {
        public static string GetDateToSqlRaw(DateTime? date)
        {
            return "" + date.Value.Year + "-" + date.Value.Month + "-" + date.Value.Day + "  00:00:00  ";
        }

        public static string GetDateToSqlRaw(int Year, int Mouth, int Day)
        {
            return "" + Year + "-" + Mouth + "-" + Day + "";
        }
        public static string GetVoucherCode(string name)
        {
            var date = DateTime.Now;
            return name + date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.Millisecond.ToString();
        }


        public static IRepositoryEF<T> ResolveRepository<T>(string ConnectionStringNames) where T : BaseEntity
        {
            return EngineContext.Current.Resolve<IRepositoryEF<T>>(ConnectionStringNames);
        }

        public static void Dump(this Exception exception)
        {
            try
            {
                exception.StackTrace.Dump();
                exception.Message.Dump();
            }
            catch
            {
            }
        }

        public static string ToAllMessages(this Exception exception, bool includeStackTrace = false)
        {
            StringBuilder stringBuilder = new StringBuilder();
            while (exception != null)
            {
                if (!stringBuilder.ToString().EmptyNull().Contains(exception.Message))
                {
                    if (includeStackTrace)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.AppendLine();
                            stringBuilder.AppendLine();
                        }

                        stringBuilder.AppendLine(exception.ToString());
                    }
                    else
                    {
                        stringBuilder.Grow(exception.Message, " * ");
                    }
                }

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }

        public static string ToElapsedMinutes(this Stopwatch watch)
        {
            return "{0:0.0}".FormatWith(TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalMinutes);
        }

        public static string ToElapsedSeconds(this Stopwatch watch)
        {
            return "{0:0.0}".FormatWith(TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalSeconds);
        }

        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            T val = default(T);
            return val.Equals(value.GetValueOrDefault());
        }

        //
        // Summary:
        //     Converts bytes into a hex string.
        public static string ToHexString(this byte[] bytes, int length = 0)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return "";
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString("x2"));
                if (length > 0 && stringBuilder.Length >= length)
                {
                    break;
                }
            }

            return stringBuilder.ToString();
        }

        //
        // Summary:
        //     Append grow if string builder is empty. Append delimiter and grow otherwise.
        //
        // Parameters:
        //   sb:
        //     Target string builder
        //
        //   grow:
        //     Value to append
        //
        //   delimiter:
        //     Delimiter to use
        public static void Grow(this StringBuilder sb, string grow, string delimiter)
        {
            Guard.NotNull(delimiter, "delimiter");
            if (!string.IsNullOrWhiteSpace(grow))
            {
                if (sb.Length <= 0)
                {
                    sb.Append(grow);
                }
                else
                {
                    sb.AppendFormat("{0}{1}", delimiter, grow);
                }
            }
        }

        public static string SafeGet(this string[] arr, int index)
        {
            return (arr != null && index < arr.Length) ? arr[index] : "";
        }
    }

    public class NullView : IView
    {
        public static readonly NullView Instance = new NullView();

        public string Path => string.Empty;

        /// <returns>A task that represents the asynchronous operation</returns>
        public Task RenderAsync(ViewContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return Task.CompletedTask;
        }
    }

    public static class ModelExtensions
    {

        public static string GetErrorsToHtml(this ResultMessageResponse result)
        {
            var errorMessages = result.errors.SelectMany(x => x.Value);

            return GetErrorsToHtml(errorMessages, result.message);
        }

        public static string GetErrorsToHtml(this ModelStateDictionary modelState, string message = null)
        {
            var errorMessages = modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);

            return GetErrorsToHtml(errorMessages, message);
        }

        public static string GetErrorsToHtml(this IEnumerable<string> errorMessages, string message = null)
        {
            var sbErrors = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(message))
                sbErrors.Append($"<span>{message}</span>");

            if (errorMessages != null && errorMessages.Any())
            {
                sbErrors.Append("<ul>");
                foreach (var errorMessage in errorMessages)
                {
                    sbErrors.Append($"<li>{errorMessage}</li>");
                }
                sbErrors.Append("</ul>");
            }

            return sbErrors.ToString();
        }

        public static Dictionary<string, IEnumerable<string>> GetErrors(this ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(w => w.Value.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.Select(s => s.ErrorMessage)
                );

            return errors;
        }
    }
}
