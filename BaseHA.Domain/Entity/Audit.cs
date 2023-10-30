﻿using System;
using System.Collections.Generic;
using BaseHA.Core.Base;

namespace BaseHA.Domain.Entity
{
    public partial class Audit: BaseEntity
    {
        public Audit()
        {
            AuditCouncils = new HashSet<AuditCouncil>();
            AuditDetails = new HashSet<AuditDetail>();
        }


        public override string Id { get; set; } = default!;


        public string VoucherCode { get; set; } = default!;


        public DateTime VoucherDate { get; set; }


        public string WareHouseId { get; set; } = default!;


        public string? Description { get; set; }


        public DateTime CreatedDate { get; set; }


        public string? CreatedBy { get; set; }


        public DateTime ModifiedDate { get; set; }


        public string? ModifiedBy { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual WareHouse WareHouse { get; set; } = null!;
        public virtual ICollection<AuditCouncil> AuditCouncils { get; set; }
        public virtual ICollection<AuditDetail> AuditDetails { get; set; }
    }
}
