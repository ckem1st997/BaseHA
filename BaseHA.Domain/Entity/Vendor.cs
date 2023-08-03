using System;
using System.Collections.Generic;
using Share.BaseCore;

#nullable disable

namespace BaseHA.Domain.Entity
{
    public partial class Vendor : BaseEntity
    {
        public Vendor()
        {
            Inwards = new HashSet<Inward>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public bool Inactive { get; set; }

        public virtual ICollection<Inward> Inwards { get; set; }
    }
}
