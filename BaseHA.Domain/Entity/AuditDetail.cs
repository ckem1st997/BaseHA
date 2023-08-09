using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class AuditDetail: BaseEntity
    {
        public AuditDetail()
        {
            AuditDetailSerials = new HashSet<AuditDetailSerial>();
        }

        public string Id { get; set; } = null!;
        public string AuditId { get; set; } = null!;
        public string? ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal AuditQuantity { get; set; }
        public string? Conclude { get; set; }
        public bool OnDelete { get; set; }

        public virtual Audit Audit { get; set; } = null!;
        public virtual WareHouseItem? Item { get; set; }
        public virtual ICollection<AuditDetailSerial> AuditDetailSerials { get; set; }
    }
}
