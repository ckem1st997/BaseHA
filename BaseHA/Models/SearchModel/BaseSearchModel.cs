namespace BaseHA.Models.SearchModel
{
    public class BaseSearchModel
    {
        public string? Keywords { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string? LanguageId { get; set; }
    }
}
