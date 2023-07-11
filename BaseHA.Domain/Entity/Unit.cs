using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage ="Bạn chưa nhập tên")]
        public string UnitName { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mã")]
        public string Code { get; set; }
        public int Number { get; set; }
        public bool Inactive { get; set; }

    }
}
