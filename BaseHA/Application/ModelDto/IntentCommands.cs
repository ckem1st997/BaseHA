using BaseHA.Domain.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseHA.Application.ModelDto
{
    public class IntentCommands : BaseCommands
    {

        public IntentCommands()
        {
            AvailableCategory = new List<SelectListItem>();
        }


        public string? IntentCodeEn { get; set; }

        public string? IntentEn { get; set; }

        public string? IntentVn { get; set; }

        public string? AnswerId { get; set; }

        public bool Inactive { get; set; }

        public virtual Answer? Answer { get; set; }
        public virtual Category? IntentCodeEnNavigation { get; set; }

        public virtual IList<SelectListItem> AvailableCategory { get; set; }
    }
}
