using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hatra.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly ISlideShowService _slideShowService;

        public SliderViewComponent(ISlideShowService slideShowService)
        {
            _slideShowService = slideShowService;
            _slideShowService.CheckArgumentIsNull(nameof(_slideShowService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModels = await _slideShowService.GetAllVisibleAsync();
            return View(viewName: "~/Views/Shared/_Slider.cshtml", viewModels);
        }
    }
}
