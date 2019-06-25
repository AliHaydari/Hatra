using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hatra.Common.GuardToolkit;
using Hatra.LuceneSearch;
using Hatra.Services.Contracts;
using Hatra.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hatra.Controllers
{
    public class SearchController : Controller
    {
        private readonly IPageService _pageService;
        private readonly ISearchManager _searchManager;

        public SearchController(ISearchManager searchManager, IPageService pageService)
        {
            _searchManager = searchManager;

            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));
        }

        //public async Task<IActionResult> Index()
        //{
        //    var pageViewModels = await _pageService.GetAllAsync();

        //    _searchManager.AddToIndex(pageViewModels.ToArray());

        //    return View();
        //}

        public async Task<IActionResult> SearchResult(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return View("Searching", new List<LuceneSearchModel>());
            }

            var res = _searchManager.Search(term, new[] { "Title", "BriefDescription", "Body" });

            ViewBag.SearchTerm = "'" + term + "'";

            return View("Searching", res);
        }
    }
}