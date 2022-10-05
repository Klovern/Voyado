using IVoyadoManagement.Search;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VoyadaManagement.Options.Search;
using System.Collections.Concurrent;

namespace VoyadaManagement.Search
{
    public class GoogleSearchManager : IGoogleSearchManager
    {
        // Not in Options due to this should be expected to go to the same endpoint
        private const string GOOGLE_API_URL = "https://www.googleapis.com/customsearch/v1?";
        private readonly IOptions<GoogleApiSettings> _options;

        // IOC the options 
        public GoogleSearchManager(IOptions<GoogleApiSettings> options)
        {
            _options = options;
        }

        public async Task<long> FetchMatchResult(IEnumerable<string> searchStrings)
        {
            // Sanitycheck input
            if (searchStrings == null) throw new ArgumentNullException(nameof(searchStrings));
            var searchStringList = searchStrings.ToList();

            // Threadsafe list of longs to avoid raiseconditions with the parallelized task executions happening per "search string"
            var bag = new ConcurrentBag<long>();

            // "Parallelized" because each "word" have a single request, trying to avoid blocking execution
            var tasks = searchStringList.Select(async search =>
            {
                var response = await ExecuteSearch(search);
                bag.Add(response);
            });

            await Task.WhenAll(tasks);
            return bag.Sum(); // Return the sum of all values
        }

        private async Task<long> ExecuteSearch(string searchString)
        {
            // Gather configuration params from IOption pattern
            var googleApiOption = _options.Value;

            // If "fields=queries(request(totalResults))" would not exist(Sends out only "amount" of found matches and not the actual data)
            // Gzip / streaming or similar compression technologies would be helpful with use of paginate through pages to avoid large requests,
            // Although it would never be a valid solution to have to paginate through the entire payload just to fetch a single "count"
            // Note I avoid using specific "nuget" packages for google api search, mainly to reduce outside dependency bloat. 
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(GOOGLE_API_URL + $"key={googleApiOption.ApiKey}&cx={googleApiOption.CX}&q={searchString}&alt=json&fields=queries(request(totalResults))");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var matches = JsonConvert.DeserializeObject<Root>(responseBody);

                if (matches == null) return 0;

                return matches.Queries?.Request?.FirstOrDefault()?.TotalResults ?? 0; // Throw or not?
            }
        }

        // Could probably something simpler / faster here since these are quite redundant and is only used to Deserialize the response from googles single result "totalResults" 
        // But i rather not look inside strings for searchtags in json manually using regex or something similar.
        #region GoogleDeserializableClass
        private class Queries
        {
            [JsonProperty("request")]
            public List<Request> Request { get; set; }
        }

        private class Request
        {
            [JsonProperty("totalResults")]
            public long TotalResults { get; set; }
        }

        private class Root
        {
            [JsonProperty("queries")]
            public Queries Queries { get; set; }
        }
        #endregion
    }
}
