using BaseHA.Domain.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseHA.Application.ModelDto
{
    public partial class VendorCommands : BaseCommands
    {

        public VendorCommands()
        {
            Inwards = new HashSet<Inward>();
            WareHouseItems = new HashSet<WareHouseItem>();
        }

        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set;}
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ContactPerson { get; set;}

        public bool Inactive { get; set; } = false; //trang thai hoat dong

        public virtual ICollection<Inward> Inwards { get; set; }
        public virtual ICollection<WareHouseItem> WareHouseItems { get; set; }

        public virtual IList<SelectListItem> AvailableWareHouses { get; set; }
    }
}
