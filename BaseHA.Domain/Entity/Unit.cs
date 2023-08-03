using System;
using System.Collections.Generic;
using Share.BaseCore;
#nullable disable

namespace BaseHA.Domain.Entity
{
    public partial class Unit : BaseEntity
    {
        public Unit()
        {
            BeginningWareHouses = new HashSet<BeginningWareHouse>();
            InwardDetails = new HashSet<InwardDetail>();
            OutwardDetails = new HashSet<OutwardDetail>();

        }

        public string UnitName { get; set; }
        public bool Inactive { get; set; }

        public virtual ICollection<BeginningWareHouse> BeginningWareHouses { get; set; }
        public virtual ICollection<InwardDetail> InwardDetails { get; set; }
        public virtual ICollection<OutwardDetail> OutwardDetails { get; set; }

    }
}
