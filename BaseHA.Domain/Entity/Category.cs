using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class Category: BaseEntity
    {
        public Category()
        {
            Answers = new HashSet<Answer>();
            Intents = new HashSet<Intent>();
        }


        public override string Id { get; set; } = default!;


        public string NameCategory { get; set; } = default!;


        public string IntentCodeEn { get; set; } = default!;


        public string IntentCodeVn { get; set; } = default!;


        public string? Description { get; set; }


        public string? ParentId { get; set; }


        public bool Inactive { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Intent> Intents { get; set; }
    }
}
