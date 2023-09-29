using System;
using System.Collections.Generic;
using BaseHA.Core.Base;

namespace BaseHA.Domain.Entity
{
    public partial class AuditCouncil: BaseEntity
    {

        public override string Id { get; set; } = default!;


        public string AuditId { get; set; } = default!;


        public string? EmployeeId { get; set; }


        public string? EmployeeName { get; set; }


        public string? Role { get; set; }


        public override bool OnDelete { get; set; } = default!;


        public virtual Audit Audit { get; set; } = null!;
    }
}
