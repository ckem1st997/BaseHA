using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class InwardDetail: BaseEntity
    {
        public InwardDetail()
        {
            SerialWareHouses = new HashSet<SerialWareHouse>();
        }


        public string Id { get; set; } = default!;


        public string InwardId { get; set; } = default!;


        public string ItemId { get; set; } = default!;


        public string UnitId { get; set; } = default!;


        public decimal Uiquantity { get; set; }


        public decimal Uiprice { get; set; }


        public decimal Amount { get; set; }


        public decimal Quantity { get; set; }


        public decimal Price { get; set; }


        public string? DepartmentId { get; set; }


        public string? DepartmentName { get; set; }


        public string? EmployeeId { get; set; }


        public string? EmployeeName { get; set; }


        public string? StationId { get; set; }


        public string? StationName { get; set; }


        public string? ProjectId { get; set; }


        public string? ProjectName { get; set; }


        public string? CustomerId { get; set; }


        public string? CustomerName { get; set; }


        public bool OnDelete { get; set; }


        public string? AccountMore { get; set; }


        public string? AccountYes { get; set; }


        public string? Status { get; set; }


    public virtual Inward Inward { get; set; } = null!;
    public virtual WareHouseItem Item { get; set; } = null!;
    public virtual Unit Unit { get; set; } = null!;
        public virtual ICollection<SerialWareHouse>
    SerialWareHouses { get; set; }
    }
}
