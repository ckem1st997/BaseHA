namespace BaseHA.Models.SearchModel
{
    public class CategorySearchModel : BaseSearchModel
    {
        public CategorySearchModel() 
        {
            Keywords = "";
        }
        public ActiveStatus ActiveStatus { get; set; }
    }
    
}
