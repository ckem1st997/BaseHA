﻿using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class WareHouse: BaseEntity
    {
        public WareHouse()
        {
            Audits = new HashSet<Audit>();
            BeginningWareHouses = new HashSet<BeginningWareHouse>();
            Inwards = new HashSet<Inward>();
            OutwardToWareHouses = new HashSet<Outward>();
            OutwardWareHouses = new HashSet<Outward>();
            WareHouseLimits = new HashSet<WareHouseLimit>();
        }


        public override string Id { get; set; } = default!;


        public string Code { get; set; } = default!;


        public string? Name { get; set; }


        public string? Address { get; set; }


        public string? Description { get; set; }


        public string? ParentId { get; set; }


        public string? Path { get; set; }


        public bool Inactive { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual ICollection<Audit> Audits { get; set; }
        public virtual ICollection<BeginningWareHouse> BeginningWareHouses { get; set; }
        public virtual ICollection<Inward> Inwards { get; set; }
        public virtual ICollection<Outward> OutwardToWareHouses { get; set; }
        public virtual ICollection<Outward> OutwardWareHouses { get; set; }
        public virtual ICollection<WareHouseLimit> WareHouseLimits { get; set; }
    }
}