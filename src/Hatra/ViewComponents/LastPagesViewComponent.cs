using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hatra.ViewComponents
{
    public class LastPagesViewComponent : ViewComponent
    {
        private readonly IPageService _pageService;

        public LastPagesViewComponent(IPageService pageService)
        {
            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModels = await _pageService.GetAllVisibleDescendingByRangeAsync(take: 12);
            return View(viewName: "~/Views/Shared/_LastPages.cshtml", viewModels);
        }
    }
}
