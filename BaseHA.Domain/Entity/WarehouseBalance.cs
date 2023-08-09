using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class WarehouseBalance: BaseEntity
    {
        public string Id { get; set; } = null!;
        public string ItemId { get; set; } = null!;
        public string WarehouseId { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public bool OnDelete { get; set; }
    }
}
