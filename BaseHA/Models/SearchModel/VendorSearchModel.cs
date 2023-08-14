namespace BaseHA.Models.SearchModel
{
    public class VendorSearchModel : BaseSearchModel    
    {
        public VendorSearchModel()
        {
            Keywords = "";
        }

        public ActiveStatus ActiveStatus { get; set; }
    }
}
