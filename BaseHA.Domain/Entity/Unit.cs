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
        }

        public string UnitName { get; set; }
        public string Code { get; set; }
        public bool Inactive { get; set; }

    }
}
