using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class AuditDetailSerial: BaseEntity
    {

        public override string Id { get; set; } = default!;


        public string ItemId { get; set; } = default!;


        public string Serial { get; set; } = default!;


        public string? AuditDetailId { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual AuditDetail? AuditDetail { get; set; }
        public virtual WareHouseItem Item { get; set; } = null!;
    }
}
