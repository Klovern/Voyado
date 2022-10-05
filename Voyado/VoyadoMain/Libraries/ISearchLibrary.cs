using VoyadoMain.Areas.Search.Models;

namespace VoyadoMain.Areas.Search.Libraries
{
    public interface ISearchLibrary
    {
        SearchResultModel FetchMatchResult(string input);
    }
}
