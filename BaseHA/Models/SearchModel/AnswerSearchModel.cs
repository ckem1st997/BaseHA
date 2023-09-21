namespace BaseHA.Models.SearchModel
{
    public class AnswerSearchModel : BaseSearchModel
    {
        public AnswerSearchModel() 
        {
            Keywords = "";
        }
        public string CategoryId { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
    }
}
