﻿using System;
using System.Collections.Generic;
using Share.BaseCore;
#nullable disable

namespace BaseHA.Domain.Entity
{
    public class Unitssss : BaseEntity
    {
        public Unitssss()
        {
        }

        public string UnitName { get; set; }
        public string Code { get; set; }
        public bool Inactive { get; set; }

    }
}
