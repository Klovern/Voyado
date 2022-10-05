using IVoyadoManagement.Search;
using VoyadoMain.Areas.Search.Models;

namespace VoyadoMain.Areas.Search.Libraries
{
    public class SearchLibrary : ISearchLibrary
    {
        private readonly IGoogleSearchManager _googleSearchManager;
        private readonly IBingSearchManager _bingSearchManager;

        public SearchLibrary(IGoogleSearchManager googleSearchManager, IBingSearchManager bingSearchManager)
        {
            _googleSearchManager = googleSearchManager;
            _bingSearchManager = bingSearchManager;
        }

        public SearchResultModel FetchMatchResult(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException("Missing inputstring", nameof(input));

            var results = new Dictionary<SearchTypeOption, SearchResultMatchProperties>();

            var splittedList = input.Trim().Split(" ").ToList();

            var googleMatches = _googleSearchManager.FetchMatchResult(splittedList);
            results.Add(SearchTypeOption.Google, new SearchResultMatchProperties()
            {
                SearchString = input,
                Matches = googleMatches.Result
            });

            var bingMatches = _bingSearchManager.FetchMatchResult(splittedList);
            results.Add(SearchTypeOption.Bing, new SearchResultMatchProperties()
            {
                SearchString = input,
                Matches = bingMatches.Result
            });

            return new SearchResultModel()
            {
                Results = results
            };
        }
    }
}
