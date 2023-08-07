using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetTopologySuite.Utilities;
using Share.BaseCore;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using System.Text;
using System.Web;
using Kendo.Mvc.UI;
using BaseHA.Models.SearchModel;

namespace BaseHA.Models
{

    public static class ModelExtensions
    {
        public static void BindRequest(this BaseSearchModel model, DataSourceRequest request)
        {
           // IWorkContext workContext = EngineContext.Current.Resolve<IWorkContext>();
            model.PageIndex = request.Page;
            model.PageSize = request.PageSize;
          //  model.LanguageId = workContext.LanguageId;
        }


        public static string GetErrorsToHtml(this ModelStateDictionary modelState, string message = null)
        {
            IEnumerable<string> errorMessages = from x in modelState.Values.SelectMany((ModelStateEntry x) => x.Errors)
                                                select x.ErrorMessage;
            return errorMessages.GetErrorsToHtml(message);
        }

        public static string GetErrorsToHtml(this IEnumerable<string> errorMessages, string message = null)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(message))
            {
                stringBuilder.Append("<span>" + message + "</span>");
            }

            if (errorMessages?.Any() ?? false)
            {
                stringBuilder.Append("<ul>");
                foreach (string errorMessage in errorMessages)
                {
                    stringBuilder.Append("<li>" + errorMessage + "</li>");
                }

                stringBuilder.Append("</ul>");
            }

            return stringBuilder.ToString();
        }

        public static Dictionary<string, IEnumerable<string>> GetErrors(this ModelStateDictionary modelState)
        {
            return modelState.Where((KeyValuePair<string, ModelStateEntry> w) => w.Value.Errors.Count > 0).ToDictionary((KeyValuePair<string, ModelStateEntry> x) => x.Key, (KeyValuePair<string, ModelStateEntry> x) => x.Value.Errors.Select((ModelError s) => s.ErrorMessage));
        }
    }


    //
    // Summary:
    //     HTML extensions
    public static class HtmlExtensions
    {
        private static readonly SelectListItem[] _singleEmptyItem = new SelectListItem[1]
        {
            new SelectListItem
            {
                Text = "",
                Value = ""
            }
        };

        public static IHtmlContent Hint(this IHtmlHelper helper, string value)
        {
            TagBuilder tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttribute("href", "#");
            tagBuilder.MergeAttribute("onclick", "return false;");
            tagBuilder.MergeAttribute("title", value);
            tagBuilder.MergeAttribute("tabindex", "-1");
            tagBuilder.AddCssClass("hint");
            TagBuilder tagBuilder2 = new TagBuilder("i");
            tagBuilder2.AddCssClass("fa fa-question-circle");
            tagBuilder.InnerHtml.AppendHtml(tagBuilder2);
            return new HtmlString(tagBuilder.ToString());
        }



        public static string FieldNameFor<T, TResult>(this IHtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            ModelExpressionProvider requiredService = html.ViewContext.HttpContext.RequestServices.GetRequiredService<ModelExpressionProvider>();
            return html.ViewData.TemplateInfo.GetFullHtmlFieldName(requiredService.GetExpressionText(expression));
        }

        public static string FieldIdFor<T, TResult>(this IHtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            ModelExpressionProvider requiredService = html.ViewContext.HttpContext.RequestServices.GetRequiredService<ModelExpressionProvider>();
            string text = html.GenerateIdFromName(html.ViewData.TemplateInfo.GetFullHtmlFieldName(requiredService.GetExpressionText(expression)));
            return text.Replace('[', '_').Replace(']', '_');
        }



       


        public static IHtmlContent MetaAcceptLanguage(this IHtmlHelper html)
        {
            string arg = HttpUtility.HtmlAttributeEncode(Thread.CurrentThread.CurrentUICulture.ToString());
            return new HtmlString($"<meta name=\"accept-language\" content=\"{arg}\"/>");
        }

      


      
 


        //
        // Summary:
        //     Convert IHtmlContent to string
        //
        // Parameters:
        //   htmlContent:
        //     HTML content
        //
        // Returns:
        //     A task that represents the asynchronous operation The task result contains the
        //     result
        public static async Task<string> RenderHtmlContentAsync(this IHtmlContent htmlContent)
        {
            await using StringWriter writer = new StringWriter();
            htmlContent.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        //
        // Summary:
        //     Convert IHtmlContent to string
        //
        // Parameters:
        //   htmlContent:
        //     HTML content
        //
        // Returns:
        //     A task that represents the asynchronous operation The task result contains the
        //     result
        public static string RenderHtmlContent(this IHtmlContent htmlContent)
        {
            using StringWriter stringWriter = new StringWriter();
            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }
    }
}
