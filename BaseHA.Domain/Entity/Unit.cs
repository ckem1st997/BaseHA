using System;
using System.Collections.Generic;
using Share.BaseCore;
#nullable disable

namespace BaseHA.Domain.Entity
{
    public class Unit : BaseEntity
    {
        public Unit()
        {
            Number = 0;
            Id = Guid.NewGuid().ToString();
        }

        public string UnitName { get; set; }
        public string Code { get; set; }
        public int Number { get; set; }
        public bool Inactive { get; set; }

    }
}
