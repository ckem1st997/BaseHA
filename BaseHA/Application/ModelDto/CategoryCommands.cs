using BaseHA.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;

namespace BaseHA.Application.ModelDto
{
    public class CategoryCommands : BaseCommands 
    {
        public CategoryCommands()
        {
            Intents = new HashSet<Intent>();
        }

        public string NameCategory { get; set; } = default!;

        
        public string IntentCodeEn { get; set; } = default!;


        public string IntentCodeVn { get; set; } = default!;


        public string? Description { get; set; }


        public string? ParentId { get; set; }


        public bool Inactive { get; set; }= false;


        public virtual ICollection<Intent> Intents { get; set; }
    }
}
