﻿using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hatra.Controllers
{
    public class ShowPageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly ICategoryService _categoryService;

        private const int DefaultPageSize = 9;

        public ShowPageController(IPageService pageService, ICategoryService categoryService)
        {
            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));

            _categoryService = categoryService;
            _categoryService.CheckArgumentIsNull(nameof(_categoryService));
        }

        [Route("page/{id:int}/{slugUrl?}")]
        public async Task<IActionResult> ShowPageDetail(int id)
        {
            var page = await _pageService.GetByIdAndUpdateViewNumberAsync(id);

            if (page == null)
            {
                return NotFound();
            }

            return View("PageDetail", page);
        }

        [Route("category/{id:int}/{slugUrl?}")]
        public async Task<IActionResult> ShowCategory(int id, int? page = 1)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var model = await _pageService.GetAllPagedVisibleByCategoryIdAsync(id, page.Value - 1, DefaultPageSize);

            model.CategoryViewModel = category;
            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            return View("PageList", model);
        }
    }
}