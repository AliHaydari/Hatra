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
        private readonly ICategoryService _categoryService;

        public ShowPageController(IPageService pageService, ICategoryService categoryService)
        {
            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));

            _categoryService = categoryService;
            _categoryService.CheckArgumentIsNull(nameof(_categoryService));
        }

        [Route("page/{id:int}")]
        public async Task<IActionResult> ShowPageWithoutCategory(int id)
        {
            var page = await _pageService.GetByIdAndUpdateViewNumberAsync(id);

            if (page == null)
            {
                return NotFound();
            }

            return View("PageDetail", page);
        }
    }
}