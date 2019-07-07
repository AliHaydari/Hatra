using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hatra.ViewComponents
{
    public class MainStaticContentViewComponent : ViewComponent
    {
        private readonly IStaticContentService _staticContentService;

        public MainStaticContentViewComponent(IStaticContentService staticContentService)
        {
            _staticContentService = staticContentService;
            _staticContentService.CheckArgumentIsNull(nameof(_staticContentService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModels = await _staticContentService.GetAllVisibleAsync();
            return View(viewName: "~/Views/Shared/_MainStaticContent.cshtml", viewModels);
        }
    }
}
