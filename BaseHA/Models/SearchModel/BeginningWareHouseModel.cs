namespace BaseHA.Models.SearchModel
{
    public class BeginningWareHouseModel : BaseSearchModel
    {
        public BeginningWareHouseModel()
        {
            Keywords = "";
        }

        public ActiveStatus ActiveStatus { get; set; }
    }
}
