namespace BaseHA.Models.SearchModel
{
    public class AnswerSearchModel : BaseSearchModel
    {
        public AnswerSearchModel() 
        {
            Keywords = "";
        }
        public ActiveStatus ActiveStatus { get; set; }
    }
}
