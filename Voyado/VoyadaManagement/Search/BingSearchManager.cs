using IVoyadoManagement.Search;
using Microsoft.Extensions.Options;
using VoyadaManagement.Options.Search;

namespace VoyadaManagement.Search
{
    public class BingSearchManager : IBingSearchManager
    {
        private const string BING_API_URL = "Url-to-bing";
        private readonly IOptions<BingApiSettings> _options;

        public BingSearchManager(IOptions<BingApiSettings> options)
        {
            _options = options;
        }

        public async Task<long> FetchMatchResult(IEnumerable<string> searchStrings)
        {
            // Do stuff use _options for inputparam and configure the api to handle the search , run webrequest
            // Retrieve the result
            // And return
            return 1;
        }
    }
}
