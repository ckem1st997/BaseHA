using BaseHA.Domain.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Share.BaseCore.IRepositories;

namespace BaseHA.Application.ModelDto
{
    public class BeginningCommands : BaseCommands
    {
        public BeginningCommands() 
        {
            AvailableWareHouses = new List<SelectListItem>();
            AvailableItem = new List<SelectListItem>();
            AvailableUnit = new List<SelectListItem>();
        }

        public string WareHouseId { get; set; }
        public string ItemId { get; set; }
        public string UnitId { get; set; }
        public string? UnitName { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        //khóa ngoại
        public virtual WareHouseItem? Item { get; set; } = null!;
        public virtual Unit? Unit { get; set; } = null!;
        public virtual WareHouse? WareHouse { get; set; } = null!;

        public virtual IList<SelectListItem> AvailableWareHouses { get; set; }
        
        public virtual IList<SelectListItem> AvailableItem { get; set; }
        public virtual IList<SelectListItem> AvailableUnit { get; set; }
    }
}
