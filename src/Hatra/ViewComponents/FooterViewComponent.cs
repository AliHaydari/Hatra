using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hatra.Common.GuardToolkit;
using Hatra.ViewModels.Identity.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hatra.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        //private readonly IMenuService _menuService;
        private readonly IOptionsSnapshot<ShowingSettingSite> _settings;

        public FooterViewComponent(/*IMenuService menuService,*/ IOptionsSnapshot<ShowingSettingSite> settings)
        {
            //_menuService = menuService;
            //_menuService.CheckArgumentIsNull(nameof(_menuService));

            _settings = settings;
            _settings.CheckArgumentIsNull(nameof(_settings));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var viewModels = await _menuService.GetAllAsync();

            var showingSettingSite = _settings.Value;
            ViewBag.Keywords = showingSettingSite.SiteKeywords;
            ViewBag.MetaDescription = showingSettingSite.Description;
            ViewBag.FooterDescription = showingSettingSite.FooterDescription;
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

            return View(viewName: "~/Views/Shared/_Footer.cshtml"/*, viewModels.Where(p => p.IsShow).ToList()*/);
        }
    }
}
