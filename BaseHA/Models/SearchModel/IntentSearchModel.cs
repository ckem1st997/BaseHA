namespace BaseHA.Models.SearchModel
{
    public class IntentSearchModel : BaseSearchModel
    {
        public IntentSearchModel()
        {
            Keywords = "";
        }
        public ActiveStatus ActiveStatus { get; set; }
        public string CategoryId { get; set; }
        
        public string Code { get; set; }    
    }
}
