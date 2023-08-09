using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class WareHouseItemUnit: BaseEntity
    {
        public string Id { get; set; } = null!;
        public string ItemId { get; set; } = null!;
        public string UnitId { get; set; } = null!;
        public int ConvertRate { get; set; }
        public bool? IsPrimary { get; set; }
        public bool OnDelete { get; set; }

        public virtual WareHouseItem Item { get; set; } = null!;
        public virtual Unit Unit { get; set; } = null!;
    }
}
