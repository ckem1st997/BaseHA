namespace BaseHA.Models.SearchModel
{
    public class CategorySearchModel : BaseSearchModel
    {
        public CategorySearchModel() 
        {
            Keywords = "";
            CategoryId = "";
        }

        public string CategoryId { get; set; }

        public ActiveStatus ActiveStatus { get; set; }
    }
    
}
