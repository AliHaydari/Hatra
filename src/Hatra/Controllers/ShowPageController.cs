using DNTBreadCrumb.Core;
using DNTCaptcha.Core;
using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Hatra.Services.Contracts.Identity;
using Hatra.ViewModels;
using Hatra.ViewModels.Identity.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Hatra.Controllers
{
    public class ShowPageController : Controller
    {
        private readonly IOptionsSnapshot<ShowingSettingSite> _settings;
        private readonly IPageService _pageService;
        private readonly ICategoryService _categoryService;
        private readonly IApplicationUserManager _applicationUserManager;
        private readonly IContactUsService _contactUsService;

        private const int DefaultPageSize = 8;

        public ShowPageController(IOptionsSnapshot<ShowingSettingSite> settings, IPageService pageService, ICategoryService categoryService, IApplicationUserManager applicationUserManager, IContactUsService contactUsService)
        {
            _settings = settings;
            _settings.CheckArgumentIsNull(nameof(_settings));

            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));

            _categoryService = categoryService;
            _categoryService.CheckArgumentIsNull(nameof(_categoryService));

            _applicationUserManager = applicationUserManager;
            _applicationUserManager.CheckArgumentIsNull(nameof(_applicationUserManager));

            _contactUsService = contactUsService;
            _contactUsService.CheckArgumentIsNull(nameof(_contactUsService));
        }

        [Route("page/{id:int}/{slugUrl?}")]
        public async Task<IActionResult> ShowPageDetail(int id, string slugUrl)
        {
            var pageViewModel = await _pageService.GetByIdAndSlugUrlAndUpdateViewNumberAsync(id, slugUrl);

            if (pageViewModel == null)
            {
                return NotFound();
            }

            var user = await _applicationUserManager.FindByIdAsync(pageViewModel.CreatedByUserId.ToString());
            pageViewModel.CreatedUserName = user.DisplayName;

            ViewBag.Keywords = pageViewModel.CategoryName;

            ViewBag.MetaDescription = pageViewModel.MetaDescription;

            ViewBag.Author = pageViewModel.CreatedUserName;

            ViewBag.LastModified = pageViewModel.CreatedDateTimeInDateTime.ToUniversalTime().ToString("ddd MMM dd yyyy HH:mm:ss \"GMT\"K");

            return View("PageDetail", pageViewModel);
        }

        [Route("category/{id:int}/{slugUrl?}")]
        public async Task<IActionResult> ShowCategory(int id, string slugUrl, int? page = 1)
        {
            var categoryViewModel = await _categoryService.GetByIdAndSlugUrlAsync(id, slugUrl);

            if (categoryViewModel == null)
            {
                return NotFound();
            }

            var model = await _pageService.GetAllPagedVisibleByCategoryIdAsync(id, page.Value - 1, DefaultPageSize);

            foreach (var pageViewModel in model.PageViewModels)
            {
                var user = await _applicationUserManager.FindByIdAsync(pageViewModel.CreatedByUserId.ToString());
                pageViewModel.CreatedUserName = user.DisplayName;
            }


            model.CategoryViewModel = categoryViewModel;
            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            return View("PageList", model);
        }

        [Route("author/{id:int}/{slugUrl?}")]
        public async Task<IActionResult> ShowPagesByUser(int id, string slugUrl, int? page = 1)
        {
            var model = await _pageService.GetAllPagedVisibleByUserIdAndSlugUrlAsync(id, slugUrl, page.Value - 1, DefaultPageSize);

            foreach (var pageViewModel in model.PageViewModels)
            {
                var user = await _applicationUserManager.FindByIdAsync(pageViewModel.CreatedByUserId.ToString());
                pageViewModel.CreatedUserName = user.DisplayName;
            }


            model.CategoryViewModel = model.CategoryViewModel;
            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            return View("PageByUserList", model);
        }



        [Route("contact-us")]
        [BreadCrumb(Title = "تماس با ما", Order = 1)]
        public IActionResult ContactUs()
        {
            var showingSettingSite = _settings.Value;
            ViewBag.Keywords = showingSettingSite.SiteKeywords;
            ViewBag.MetaDescription = showingSettingSite.Description;
            ViewBag.PersianSiteName = showingSettingSite.PersianSiteName;
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
            ViewBag.Latitude = showingSettingSite.Latitude;
            ViewBag.Longitude = showingSettingSite.Longitude;

            return View(new ContactUsViewModel());
        }

        [Route("contact-us")]
        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ثبت تماس")]
        [ValidateDNTCaptcha(CaptchaGeneratorLanguage = DNTCaptcha.Core.Providers.Language.Persian)]
        public async Task<IActionResult> ContactUs(ContactUsViewModel viewModel)
        {
            var showingSettingSite = _settings.Value;
            ViewBag.Keywords = showingSettingSite.SiteKeywords;
            ViewBag.MetaDescription = showingSettingSite.Description;
            ViewBag.PersianSiteName = showingSettingSite.PersianSiteName;
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
            ViewBag.Latitude = showingSettingSite.Latitude;
            ViewBag.Longitude = showingSettingSite.Longitude;

            if (ModelState.IsValid)
            {
                var result = await _contactUsService.InsertAsync(viewModel);
                if (result)
                {
                    return View(new ContactUsViewModel());
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        //[Route("about-us")]
        //[BreadCrumb(Title = "تماس با ما", Order = 1)]
        //public IActionResult AboutUs()
        //{
        //    var showingSettingSite = _settings.Value;
        //    ViewBag.Keywords = showingSettingSite.SiteKeywords;
        //    ViewBag.MetaDescription = showingSettingSite.Description;
        //    ViewBag.PersianSiteName = showingSettingSite.PersianSiteName;
        //    ViewBag.WorkTime = showingSettingSite.WorkTime;
        //    ViewBag.Tell1 = showingSettingSite.Tell1;
        //    ViewBag.Tell2 = showingSettingSite.Tell2;
        //    ViewBag.Email = showingSettingSite.Email;
        //    ViewBag.Address = showingSettingSite.Address;
        //    ViewBag.Twitter = showingSettingSite.Twitter;
        //    ViewBag.Facebook = showingSettingSite.Facebook;
        //    ViewBag.Skype = showingSettingSite.Skype;
        //    ViewBag.Pinterest = showingSettingSite.Pinterest;
        //    ViewBag.Telegram = showingSettingSite.Telegram;
        //    ViewBag.Instagram = showingSettingSite.Instagram;
        //    ViewBag.LinkedIn = showingSettingSite.LinkedIn;
        //    ViewBag.WhatsApp = showingSettingSite.WhatsApp;

        //    return View("AboutUs");
        //}
    }
}