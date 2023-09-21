using BaseHA.Domain.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Share.BaseCore;

namespace BaseHA.Application.ModelDto
{
    public class CategoryCommands : BaseCommands
    {
        public CategoryCommands()
        {
            Answers = new List<AnswerCommands>();
            Intents = new List<IntentCommands>();
            AvailableCaegorys = new List<SelectListItem>();
        }


        public string NameCategory { get; set; } = default!;


        public string IntentCodeEn { get; set; } = default!;


        public string IntentCodeVn { get; set; } = default!;


        public string? Description { get; set; }


        public string? ParentId { get; set; }


        public bool Inactive { get; set; }


        public virtual ICollection<AnswerCommands> Answers { get; set; }
        public virtual ICollection<IntentCommands> Intents { get; set; }
        public virtual IList<SelectListItem> AvailableCaegorys { get; set; }

    }
}
