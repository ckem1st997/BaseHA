namespace BaseHA.Models.SearchModel
{
    public class CategoryTbSearchModel : BaseSearchModel
    {
        public CategoryTbSearchModel()
        {
            Keywords = "";
        }

        public ActiveStatus ActiveStatus { get; set; }

    }
}
