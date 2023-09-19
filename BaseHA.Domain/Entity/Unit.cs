﻿using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class Unit: BaseEntity
    {
        public Unit()
        {
            BeginningWareHouses = new HashSet<BeginningWareHouse>();
            InwardDetails = new HashSet<InwardDetail>();
            OutwardDetails = new HashSet<OutwardDetail>();
            WareHouseItemUnits = new HashSet<WareHouseItemUnit>();
            WareHouseItems = new HashSet<WareHouseItem>();
            WareHouseLimits = new HashSet<WareHouseLimit>();
        }


        public override string Id { get; set; } = default!;


        public string? UnitName { get; set; }


        public bool Inactive { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual ICollection<BeginningWareHouse> BeginningWareHouses { get; set; }
        public virtual ICollection<InwardDetail> InwardDetails { get; set; }
        public virtual ICollection<OutwardDetail> OutwardDetails { get; set; }
        public virtual ICollection<WareHouseItemUnit> WareHouseItemUnits { get; set; }
        public virtual ICollection<WareHouseItem> WareHouseItems { get; set; }
        public virtual ICollection<WareHouseLimit> WareHouseLimits { get; set; }
    }
}