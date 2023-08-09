using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class BeginningWareHouse: BaseEntity
    {
        public string Id { get; set; } = null!;
        public string WareHouseId { get; set; } = null!;
        public string ItemId { get; set; } = null!;
        public string UnitId { get; set; } = null!;
        public string? UnitName { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public bool OnDelete { get; set; }

        public virtual WareHouseItem Item { get; set; } = null!;
        public virtual Unit Unit { get; set; } = null!;
        public virtual WareHouse WareHouse { get; set; } = null!;
    }
}
