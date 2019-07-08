using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Hatra.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hatra.ViewComponents
{
    public class SidebarWidgetViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IPageService _pageService;

        public SidebarWidgetViewComponent(ICategoryService categoryService, IPageService pageService)
        {
            _categoryService = categoryService;
            _categoryService.CheckArgumentIsNull(nameof(_categoryService));

            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));
        }

        public async Task<IViewComponentResult> InvokeAsync(int? activeCategoryId)
        {
            var categoryViewModels = await _categoryService.GetAllVisibleSidebarWidgetAsync();

            var pageViewModels = await _pageService.GetAllLastContentVisibleDescendingByRangeAsync(take: 4);

            var viewModel = new SidebarWidgetViewModel()
            {
                ActiveCategoryId = activeCategoryId,
                CategoryViewModels = categoryViewModels,
                LastPageViewModels = pageViewModels,
            };

            return View(viewName: "~/Views/Shared/_SidebarWidget.cshtml", viewModel);
        }
    }
}
