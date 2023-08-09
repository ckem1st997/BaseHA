using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class AuditCouncil: BaseEntity
    {
        public string Id { get; set; } = null!;
        public string AuditId { get; set; } = null!;
        public string? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? Role { get; set; }
        public bool OnDelete { get; set; }

        public virtual Audit Audit { get; set; } = null!;
    }
}
