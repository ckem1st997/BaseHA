using BaseHA.Domain.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseHA.Application.ModelDto
{
    public partial class WareHouseCommands : BaseCommands
    {
        public WareHouseCommands()
        {
            Audits = new HashSet<Audit>();
            BeginningWareHouses = new HashSet<BeginningWareHouse>();
            Inwards = new HashSet<Inward>();
            OutwardToWareHouses = new HashSet<Outward>();
            OutwardWareHouses = new HashSet<Outward>();
            WareHouseLimits = new HashSet<WareHouseLimit>();
            AvailableWareHouses = new List<SelectListItem>();
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public string Path { get; set; }
        public bool Inactive { get; set; } = false;

        public virtual ICollection<Audit> Audits { get; set; }
        public virtual ICollection<BeginningWareHouse> BeginningWareHouses { get; set; }
        public virtual ICollection<Inward> Inwards { get; set; }
        public virtual ICollection<Outward> OutwardToWareHouses { get; set; }
        public virtual ICollection<Outward> OutwardWareHouses { get; set; }
        public virtual ICollection<WareHouseLimit> WareHouseLimits { get; set; }
        public virtual IList<SelectListItem> AvailableWareHouses { get; set; }
    }


}
