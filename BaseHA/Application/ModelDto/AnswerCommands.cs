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

        public string IntentId { get; set; } = default!;


        public string? IntentCodeEn { get; set; }


        public string? AnswerVn { get; set; }


        public bool Inactive { get; set; }


        public virtual Intent? Intent { get; set; }
    }
}
