using BaseHA.Domain.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseHA.Application.ModelDto
{
    public class IntentCommands
    {

        public IntentCommands()
        {
    
        }
        public string Id { get; set; } 

        public string? IntentCodeEn { get; set; }

        public string? IntentEn { get; set; }

        public string? IntentVn { get; set; }

        public bool Inactive { get; set; }
        public bool Ondelete { get; set; }


        public virtual Category? IntentCodeEnNavigation { get; set; }

    }
}
