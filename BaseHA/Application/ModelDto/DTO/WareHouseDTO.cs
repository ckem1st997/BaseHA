using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaseHA.Application.ModelDto.DTO
{
    public class BaseModel
    {
        public string Id { get; set; }
        public bool OnDelete { get; set; }
    }
    public class WareHouseDTO : BaseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public string Path { get; set; }
        public bool Inactive { get; set; }

        public virtual IEnumerable<WareHouseDTO> WareHouseDTOs { get; set; }

    }
}
