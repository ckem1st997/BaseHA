namespace BaseHA.Models
{
    public class UnitSearchModel : BaseSearchModel
    {
        public UnitSearchModel()
        {
            this.Keywords = "";
        }

        public ActiveStatus ActiveStatus { get; set; }

    }


   

    
}
