using BaseHA.Domain.Entity;
using Share.BaseCore;

namespace BaseHA.Application.ModelDto
{
    public class AnswerCommands : BaseCommands
    {

        public string IntentCodeEn { get; set; } = default!;


        public string? AnswerVn { get; set; }


        public bool Inactive { get; set; }

        public virtual Category IntentCodeEnNavigation { get; set; } = null!;
    }
}
