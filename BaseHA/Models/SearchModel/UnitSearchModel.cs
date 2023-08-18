namespace BaseHA.Models.SearchModel
{
    public class UnitSearchModel : BaseSearchModel
    {
        public UnitSearchModel()
        {
            Keywords = "";
        }

        public ActiveStatus ActiveStatus { get; set; }
    }
}
