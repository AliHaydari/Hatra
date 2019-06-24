using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Hatra.Services.Contracts.Identity;

namespace Hatra.Controllers
{
    public class ShowPageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly ICategoryService _categoryService;
        private readonly IApplicationUserManager _applicationUserManager;

        private const int DefaultPageSize = 9;

        public ShowPageController(IPageService pageService, ICategoryService categoryService, IApplicationUserManager applicationUserManager)
        {
            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));

            _categoryService = categoryService;
            _categoryService.CheckArgumentIsNull(nameof(_categoryService));

            _applicationUserManager = applicationUserManager;
            _applicationUserManager.CheckArgumentIsNull(nameof(_applicationUserManager));
        }

        [Route("page/{id:int}/{slugUrl?}")]
        public async Task<IActionResult> ShowPageDetail(int id, string slugUrl)
        {
            var pageViewModel = await _pageService.GetByIdAndSlugUrlAndUpdateViewNumberAsync(id, slugUrl);

            if (pageViewModel == null)
            {
                return NotFound();
            }

            var user = await _applicationUserManager.FindByIdAsync(pageViewModel.CreatedByUserId.ToString());
            pageViewModel.CreatedUserName = user.DisplayName;

            return View("PageDetail", pageViewModel);
        }

        [Route("category/{id:int}/{slugUrl?}")]
        public async Task<IActionResult> ShowCategory(int id, string slugUrl, int? page = 1)
        {
            var categoryViewModel = await _categoryService.GetByIdAndSlugUrlAsync(id, slugUrl);

            if (categoryViewModel == null)
            {
                return NotFound();
            }

            var model = await _pageService.GetAllPagedVisibleByCategoryIdAsync(id, page.Value - 1, DefaultPageSize);

            foreach (var pageViewModel in model.PageViewModels)
            {
                var user = await _applicationUserManager.FindByIdAsync(pageViewModel.CreatedByUserId.ToString());
                pageViewModel.CreatedUserName = user.DisplayName;
            }


            model.CategoryViewModel = categoryViewModel;
            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            return View("PageList", model);
        }

        [Route("author/{id:int}/{slugUrl?}")]
        public async Task<IActionResult> ShowPagesByUser(int id, string slugUrl, int? page = 1)
        {
            var model = await _pageService.GetAllPagedVisibleByUserIdAndSlugUrlAsync(id, slugUrl, page.Value - 1, DefaultPageSize);

            foreach (var pageViewModel in model.PageViewModels)
            {
                var user = await _applicationUserManager.FindByIdAsync(pageViewModel.CreatedByUserId.ToString());
                pageViewModel.CreatedUserName = user.DisplayName;
            }


            model.CategoryViewModel = model.CategoryViewModel;
            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            return View("PageByUserList", model);
        }
    }
}