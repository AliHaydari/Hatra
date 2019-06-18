using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Hatra.Controllers
{
    public class ShowPageController : Controller
    {
        private readonly IPageService _pageService;

        public ShowPageController(IPageService pageService)
        {
            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));
        }

        public async Task<IActionResult> Index(int id)
        {
            var page = await _pageService.GetByIdAndUpdateViewNumberAsync(id);

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }
    }
}