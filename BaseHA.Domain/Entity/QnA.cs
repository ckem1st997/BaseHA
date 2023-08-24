using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class QnA: BaseEntity
    {

        public override string Id { get; set; } = default!;


        public string Question { get; set; } = default!;


        public string Answer { get; set; } = default!;


        public override bool OnDelete { get; set; } = default!;

    }
}
