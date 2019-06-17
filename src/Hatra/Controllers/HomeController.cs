using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Hatra.Common.GuardToolkit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hatra.Common.IdentityToolkit;
using Hatra.Services.Contracts;
using Hatra.ViewModels;

namespace Hatra.Controllers
{
    [BreadCrumb(Title = "خانه", UseDefaultRouteUrl = true, Order = 0)]
    public class HomeController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly ISlideShowService _slideShowService;

        public HomeController(IMenuService menuService, ISlideShowService slideShowService)
        {
            _menuService = menuService;
            _menuService.CheckArgumentIsNull(nameof(_menuService));

            _slideShowService = slideShowService;
            _slideShowService.CheckArgumentIsNull(nameof(_slideShowService));
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var menuViewModels = await _menuService.GetAllAsync();
            var slideShowViewModels = await _slideShowService.GetAllAsync();

            var viewModel = new HomeViewModel()
            {
                MenuViewModels = menuViewModels,
                SlideShowViewModels = slideShowViewModels,
            };

            return View("IndexN", viewModel);
        }

        [BreadCrumb(Title = "خطا", Order = 1)]
        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// To test automatic challenge after redirecting from another site
        /// Sample URL: http://localhost:5000/Home/CallBackResult?token=1&status=2&orderId=3&terminalNo=4&rrn=5
        /// </summary>
        [Authorize]
        public IActionResult CallBackResult(long token, string status, string orderId, string terminalNo, string rrn)
        {
            var userId = User.Identity.GetUserId();
            return Json(new { userId, token, status, orderId, terminalNo, rrn });
        }
    }
}