using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class WarehouseBalance: BaseEntity
    {

        public string Id { get; set; } = default!;


        public string ItemId { get; set; } = default!;


        public string WarehouseId { get; set; } = default!;


        public decimal Quantity { get; set; }


        public decimal Amount { get; set; }


        public bool OnDelete { get; set; }

    }
}
