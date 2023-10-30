namespace BaseHA.Models.SearchModel
{
    public class WareHouseSearchModel : BaseSearchModel
    {
        public WareHouseSearchModel()
        {
            Keywords = "";
            WareHouseId = "";
        }
        public string WareHouseId { get; set; }
    }

    public class UnitSearchModel : BaseSearchModel
    {
        public UnitSearchModel()
        {
            Keywords = "";
        }
    }

    public class CategorySearchModel : BaseSearchModel
    {
        public CategorySearchModel()
        {
            Keywords = "";
        }
        public string CategoryId { get; set; }
    }  
    
    public class IntentSearchModel : BaseSearchModel
    {
        public IntentSearchModel()
        {
            Keywords = "";
        }
        public string CategoryId { get; set; }
    }

   
    public class AnswerSearchModel : BaseSearchModel
    {
        public AnswerSearchModel()
        {
            Keywords = "";
        }
        public string CategoryId { get; set; }
    }



}
