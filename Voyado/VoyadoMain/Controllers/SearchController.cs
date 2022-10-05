using Microsoft.AspNetCore.Mvc;
using VoyadoMain.Areas.Search.Libraries;
using VoyadoMain.Models;

namespace VoyadoMain.Areas.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchLibrary _searchLibrary;

        public SearchController(ISearchLibrary searchLibrary)
        {
            _searchLibrary = searchLibrary;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetSearchMatches(string input)
        {
            var model = _searchLibrary.FetchMatchResult(input);
            var searchInput = new SearchInput()
            {
                Input = input,
                Result = model
            };
            return View("index", searchInput);
        }
    }
}
