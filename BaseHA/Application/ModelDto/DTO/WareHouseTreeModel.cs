using Share.BaseCore.Base;

namespace BaseHA.Application.ModelDto.DTO
{
    public class WareHouseTreeModel : FancytreeItem
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string ParentId { get; set; }

        public string Path { get; set; }

        public string Description { get; set; }

        public bool Inactive { get; set; }

        public new IList<WareHouseTreeModel> children { get; set; }

        public WareHouseTreeModel()
        {
            children = new List<WareHouseTreeModel>();
        }
    }
}
