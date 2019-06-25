using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hatra.LuceneSearch
{
    public class SearchModel : PageModel
    {
        public SearchResultCollection Results { get; set; }
        [BindProperty(SupportsGet = true)] public string Search { get; set; }

        public void OnGet()
        {
            Results = new SearchResultCollection();
        }
    }
}
