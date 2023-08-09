using System;
using System.Collections.Generic;
using Share.BaseCore;

namespace BaseHA.Domain.Entity
{
    public partial class VWareHouseLedger: BaseEntity
    {

        public string Id { get; set; } = default!;


        public string WareHouseId { get; set; } = default!;


        public string ItemId { get; set; } = default!;


        public string UnitId { get; set; } = default!;


        public decimal? Quantity { get; set; }


        public string? VoucherCode { get; set; }


        public DateTime? VoucherDate { get; set; }


        public string? DepartmentId { get; set; }


        public string? DepartmentName { get; set; }


        public string? StationId { get; set; }


        public string? StationName { get; set; }


        public string? CustomerId { get; set; }


        public string? CustomerName { get; set; }


        public string? ProjectId { get; set; }


        public string? ProjectName { get; set; }


        public string? EmployeeId { get; set; }


        public string? EmployeeName { get; set; }

    }
}
