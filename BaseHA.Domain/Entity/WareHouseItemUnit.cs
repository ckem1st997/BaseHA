using System;
using System.Collections.Generic;
using BaseHA.Core.Base;

namespace BaseHA.Domain.Entity
{
    public partial class WareHouseItemUnit: BaseEntity
    {

        public override string Id { get; set; } = default!;


        public string ItemId { get; set; } = default!;


        public string UnitId { get; set; } = default!;


        public int ConvertRate { get; set; }


        public bool? IsPrimary { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual WareHouseItem Item { get; set; } = null!;
        public virtual Unit Unit { get; set; } = null!;
    }
}
