using BaseHA.Domain.Entity;
using Share.BaseCore;

namespace BaseHA.Application.ModelDto
{
    public class IntentCommands : BaseCommands
    {

        public string? CategoryId { get; set; }


        public string? IntentEn { get; set; }


        public string? IntentVn { get; set; }


        public bool Inactive { get; set; }



        public virtual CategoryCommands? CategoryCommands { get; set; }
    }
}
