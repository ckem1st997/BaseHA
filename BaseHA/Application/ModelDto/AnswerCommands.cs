using BaseHA.Domain.Entity;
using Share.BaseCore;

namespace BaseHA.Application.ModelDto
{
    public class AnswerCommands : BaseCommands
    {

        public string? CategoryId { get; set; }


        public string? AnswerVn { get; set; }


        public bool Inactive { get; set; }

        public virtual CategoryCommands? CategoryCommands { get; set; }
    }
}
