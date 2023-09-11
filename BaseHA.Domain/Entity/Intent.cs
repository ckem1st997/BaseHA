using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class Intent: BaseEntity
    {

        public override string Id { get; set; } = default!;


        public string? IntentCodeEn { get; set; }


        public string? IntentEn { get; set; }


        public string? IntentVn { get; set; }


        public string? CategoryId { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual CategoryTb? Category { get; set; }
    }
}
