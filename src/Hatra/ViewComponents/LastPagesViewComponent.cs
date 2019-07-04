using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Hatra.Services.Contracts.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hatra.ViewComponents
{
    public class LastPagesViewComponent : ViewComponent
    {
        private readonly IPageService _pageService;
        private readonly IApplicationUserManager _applicationUserManager;

        public LastPagesViewComponent(IPageService pageService, IApplicationUserManager applicationUserManager)
        {
            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));

            _applicationUserManager = applicationUserManager;
            _applicationUserManager.CheckArgumentIsNull(nameof(_applicationUserManager));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModels = await _pageService.GetAllLastContentVisibleDescendingByRangeAsync(take: 12);

            foreach (var pageViewModel in viewModels)
            {
                var user = await _applicationUserManager.FindByIdAsync(pageViewModel.CreatedByUserId.ToString());
                pageViewModel.CreatedUserName = user.DisplayName;
            }

            return View(viewName: "~/Views/Shared/_LastPages.cshtml", viewModels);
        }
    }
}
