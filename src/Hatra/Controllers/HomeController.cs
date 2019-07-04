using DNTBreadCrumb.Core;
using Hatra.Common.GuardToolkit;
using Hatra.Common.IdentityToolkit;
using Hatra.Services.Contracts;
using Hatra.ViewModels.Identity.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hatra.Controllers
{
    [BreadCrumb(Title = "خانه", UseDefaultRouteUrl = true, Order = 0)]
    public class HomeController : Controller
    {
        private readonly IContactUsService _contactUsService;
        private readonly IOptionsSnapshot<ShowingSettingSite> _settings;

        public HomeController(IContactUsService contactUsService, IOptionsSnapshot<ShowingSettingSite> settings)
        {
            _contactUsService = contactUsService;
            _contactUsService.CheckArgumentIsNull(nameof(_contactUsService));

            _settings = settings;
            _settings.CheckArgumentIsNull(nameof(_settings));
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {
            var showingSettingSite = _settings.Value;
            ViewBag.Keywords = showingSettingSite.SiteKeywords;
            ViewBag.MetaDescription = showingSettingSite.Description;
            ViewBag.SiteName = showingSettingSite.PersianSiteName;
            ViewBag.WorkTime = showingSettingSite.WorkTime;
            ViewBag.Tell1 = showingSettingSite.Tell1;
            ViewBag.Tell2 = showingSettingSite.Tell2;
            ViewBag.Email = showingSettingSite.Email;
            ViewBag.Address = showingSettingSite.Address;
            ViewBag.Twitter = showingSettingSite.Twitter;
            ViewBag.Facebook = showingSettingSite.Facebook;
            ViewBag.Skype = showingSettingSite.Skype;
            ViewBag.Pinterest = showingSettingSite.Pinterest;
            ViewBag.Telegram = showingSettingSite.Telegram;
            ViewBag.Instagram = showingSettingSite.Instagram;
            ViewBag.LinkedIn = showingSettingSite.LinkedIn;
            ViewBag.WhatsApp = showingSettingSite.WhatsApp;

            return View("IndexN");
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