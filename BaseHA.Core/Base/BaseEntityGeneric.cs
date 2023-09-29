﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseHA.Core.Base
{
    public abstract class BaseEntityGeneric<T>
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        [Key]
        public abstract T Id { get; set; }
    }
    public interface IBaseEntityGeneric<T>
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        [Key]
        public T Id { get; set; }
    }
}