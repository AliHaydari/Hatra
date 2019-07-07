using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Hatra.ViewComponents
{
    public class BrandViewComponent : ViewComponent
    {
        private readonly IBrandService _brandService;

        public BrandViewComponent(IBrandService brandService)
        {
            _brandService = brandService;
            _brandService.CheckArgumentIsNull(nameof(_brandService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModels = await _brandService.GetAllVisibleAsync();
            return View(viewName: "~/Views/Shared/_Brand.cshtml", viewModels.Where(p => p.IsShow).ToList());
        }
    }
}
