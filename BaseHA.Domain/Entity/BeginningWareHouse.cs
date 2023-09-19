﻿using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class BeginningWareHouse: BaseEntity
    {

        public override string Id { get; set; } = default!;


        public string WareHouseId { get; set; } = default!;


        public string ItemId { get; set; } = default!;


        public string UnitId { get; set; } = default!;


        public string? UnitName { get; set; }


        public decimal Quantity { get; set; }


        public DateTime CreatedDate { get; set; }


        public string? CreatedBy { get; set; }


        public DateTime ModifiedDate { get; set; }


        public string? ModifiedBy { get; set; }


        public override bool OnDelete { get; set; } = default!;

        
        public virtual WareHouseItem? Item { get; set; } = null!;
        public virtual Unit? Unit { get; set; } = null!;
        public virtual WareHouse? WareHouse { get; set; } = null!;
        
    }
}