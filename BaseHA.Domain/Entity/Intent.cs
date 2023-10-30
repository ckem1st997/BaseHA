using System;
using System.Collections.Generic;
using BaseHA.Core.Base;

namespace BaseHA.Domain.Entity
{
    public partial class Intent: BaseEntity
    {

        public override string Id { get; set; } = default!;


        public string? CategoryId { get; set; }


        public string? IntentEn { get; set; }


        public string? IntentVn { get; set; }


        public bool Inactive { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual Category? Category { get; set; }
    }
}
