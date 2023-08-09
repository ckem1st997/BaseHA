using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class Audit: BaseEntity
    {
        public Audit()
        {
            AuditCouncils = new HashSet<AuditCouncil>();
            AuditDetails = new HashSet<AuditDetail>();
        }

        public string Id { get; set; } = null!;
        public string VoucherCode { get; set; } = null!;
        public DateTime VoucherDate { get; set; }
        public string WareHouseId { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public bool OnDelete { get; set; }

        public virtual WareHouse WareHouse { get; set; } = null!;
        public virtual ICollection<AuditCouncil> AuditCouncils { get; set; }
        public virtual ICollection<AuditDetail> AuditDetails { get; set; }
    }
}
