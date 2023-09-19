using BaseHA.Domain.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseHA.Application.ModelDto
{
    public class AnswerCommands : BaseCommands
    {
        public AnswerCommands()
        {

        }

        public string IntentCodeEn { get; set; } = default!;

        public string? AnswerVn { get; set; }

        public bool Inactive { get; set; } = false;

        public virtual Category IntentCodeEnNavigation { get; set; } 
    }
}
