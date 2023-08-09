using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class SerialWareHouse: BaseEntity
    {

        public string Id { get; set; } = default!;


        public string ItemId { get; set; } = default!;


        public string Serial { get; set; } = default!;


        public string? InwardDetailId { get; set; }


        public string? OutwardDetailId { get; set; }


        public bool IsOver { get; set; }


        public bool OnDelete { get; set; }


    public virtual InwardDetail? InwardDetail { get; set; }
    public virtual WareHouseItem Item { get; set; } = null!;
    public virtual OutwardDetail? OutwardDetail { get; set; }
    }
}
