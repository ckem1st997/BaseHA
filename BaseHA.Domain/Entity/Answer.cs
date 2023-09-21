using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class Answer: BaseEntity
    {

        public override string Id { get; set; } = default!;


        public string IntentCodeEn { get; set; } = default!;


        public string? AnswerVn { get; set; }


        public bool Inactive { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual Category IntentCodeEnNavigation { get; set; } = null!;
    }
}
