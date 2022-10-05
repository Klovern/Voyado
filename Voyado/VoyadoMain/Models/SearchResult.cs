namespace VoyadoMain.Areas.Search.Models
{
    public class SearchResultModel
    {
        public Dictionary<SearchTypeOption, SearchResultMatchProperties> Results { get; set; }
    }

    public class SearchResultMatchProperties
    {
        public string SearchString { get; set; }
        public long Matches { get; set; }
    }
}
