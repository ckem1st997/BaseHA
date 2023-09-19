using Share.BaseCore.Base;

namespace BaseHA.Application.ModelDto.DTO
{
    public class CategoryTreeModel : FancytreeItem
    {
        public string NameCategory { get; set; }
        public string IntentCodeEn { get; set; }
        public string IntentCodeVn { get; set; }
        public string? Description { get; set; }
        public string? ParentId { get; set; }
        public bool Inactive { get; set; }

        public new IList<CategoryTreeModel> children { get; set; }

        public CategoryTreeModel() 
        {
            children = new List<CategoryTreeModel>();
        }
    }
}
