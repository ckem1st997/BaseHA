using BaseHA.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace BaseHA.Core.Filters
{
    /// <summary>
    /// custom reponse message to validator by flutent
    /// </summary>
    public class CustomValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage)
                    .ToList();
                var errorsString = errors.AnyList() ? string.Join("|", errors) : "Đã xảy ra lỗi với dữ liệu đầu vào !";
                var responseObj = new ResultMessageResponse
                {
                    //code = "200",
                    code = "500",
                    message = errorsString,
                    errors = new Dictionary<string, IEnumerable<string>>()
                    {
                        {
                            "msg",
                            errors
                        }
                    },
                    success = false
                };

                context.Result = new JsonResult(responseObj)
                {
                    //StatusCode = 200
                    StatusCode = 500
                };
            }
        }
    }
}