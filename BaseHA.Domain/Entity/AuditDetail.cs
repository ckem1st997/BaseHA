using System;
using System.Collections.Generic;
using BaseHA.Core.Base;

namespace BaseHA.Domain.Entity
{
    public partial class AuditDetail: BaseEntity
    {
        public AuditDetail()
        {
            AuditDetailSerials = new HashSet<AuditDetailSerial>();
        }


        public override string Id { get; set; } = default!;


        public string AuditId { get; set; } = default!;


        public string? ItemId { get; set; }


        public decimal Quantity { get; set; }


        public decimal AuditQuantity { get; set; }


        public string? Conclude { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual Audit Audit { get; set; } = null!;
        public virtual WareHouseItem? Item { get; set; }
        public virtual ICollection<AuditDetailSerial> AuditDetailSerials { get; set; }
    }
}
