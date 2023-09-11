using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class CategoryTb: BaseEntity
    {
        public CategoryTb()
        {
            Intents = new HashSet<Intent>();
        }


        public override string Id { get; set; } = default!; //Guid.NewGuid().ToString();


        public string Category { get; set; } = default!;


        public string? IntentCodeEn { get; set; }


        public string? IntentCodeVn { get; set; }


        public string? Description { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual ICollection<Intent> Intents { get; set; }
    }
}
