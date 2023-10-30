using BaseHA.Core.Base;
using BaseHA.Core.Extensions;
using BaseHA.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseHA.Core.Filters
{
    public class NotifyAttribute : ActionFilterAttribute
    {
        public const string NotificationsKey = "sm.notifications.all";

       // public INotifier Notifier { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var Notifier = EngineContext.Current.Resolve<INotifier>();

            if (Notifier == null || Notifier.Entries.Count == 0)
                return;
            var controller = (Controller)filterContext.Controller;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                HandleAjaxRequest(Notifier.Entries.FirstOrDefault(), filterContext.HttpContext.Response);
                return;
            }
            Persist(controller.ViewData, Notifier.Entries.Where(x => x.Durable == false));
            Persist(controller.TempData, Notifier.Entries.Where(x => x.Durable == true));

            Notifier.Entries.Clear();
        }

        public virtual void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        private void Persist(IDictionary<string, object> bag, IEnumerable<NotifyEntry> source)
        {
            if (!source.Any())
                return;

            var existing = (bag[NotificationsKey] ?? new HashSet<NotifyEntry>()) as HashSet<NotifyEntry>;

            source.Each(x =>
            {
                if (x.Message.HasValue())
                    existing?.Add(x);
            });

            bag[NotificationsKey] = TrimSet(existing);
        }

        private void HandleAjaxRequest(NotifyEntry entry, HttpResponse response)
        {
            if (entry == null)
                return;

            response.Headers.Add("X-Message-Type", entry.Type.ToString().ToLower());
            response.Headers.Add("X-Message", Convert.ToBase64String(Encoding.UTF8.GetBytes(entry.Message)));
        }

        private HashSet<NotifyEntry> TrimSet(HashSet<NotifyEntry> entries)
        {
            if (entries.Count <= 20)
            {
                return entries;
            }

            return new HashSet<NotifyEntry>(entries.Skip(entries.Count - 20));
        }
    }

}
