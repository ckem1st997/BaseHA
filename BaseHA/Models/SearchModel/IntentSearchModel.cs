namespace BaseHA.Models.SearchModel
{
    public class IntentSearchModel : BaseSearchModel
    {
        public IntentSearchModel()
        {
            Keywords = "";
        }
        public ActiveStatus ActiveStatus { get; set; }
        
        public String Code { get; set; }    
    }
}
