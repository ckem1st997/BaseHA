using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class WareHouseItemUnit: BaseEntity
    {

        public string Id { get; set; } = default!;


        public string ItemId { get; set; } = default!;


        public string UnitId { get; set; } = default!;


        public int ConvertRate { get; set; }


        public bool? IsPrimary { get; set; }


        public bool OnDelete { get; set; }


    public virtual WareHouseItem Item { get; set; } = null!;
    public virtual Unit Unit { get; set; } = null!;
    }
}
