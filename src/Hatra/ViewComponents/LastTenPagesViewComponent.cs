using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hatra.ViewComponents
{
    public class LastTenPagesViewComponent : ViewComponent
    {
        private readonly IPageService _pageService;

        public LastTenPagesViewComponent(IPageService pageService)
        {
            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModels = await _pageService.GetLastRecordAsync();
            return View(viewName: "~/Views/Shared/_Last10Pages.cshtml", viewModels);
        }
    }
}
