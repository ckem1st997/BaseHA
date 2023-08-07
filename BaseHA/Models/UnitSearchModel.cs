namespace BaseHA.Models
{
    public class WareHouseSearchModel : BaseSearchModel
    {
        public WareHouseSearchModel()
        {
            this.Keywords = "";
        }

        public ActiveStatus ActiveStatus { get; set; }

    }





}
