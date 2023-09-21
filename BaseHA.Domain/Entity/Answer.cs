using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class Answer: BaseEntity
    {

        public override string Id { get; set; } = default!;


        public string? CategoryId { get; set; }


        public string? AnswerVn { get; set; }


        public bool Inactive { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual Category? Category { get; set; }
    }
}
