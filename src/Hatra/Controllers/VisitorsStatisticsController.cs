using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Hatra.Common.GuardToolkit;
using Hatra.Common.WebToolkit;
using Hatra.Services.Contracts;
using Hatra.Services.Identity;
using Hatra.ViewModels.VisitorsStatistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hatra.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("آمار بازدید سایت")]
    public class VisitorsStatisticsController : Controller
    {
        private readonly IVisitorsStatisticsService _visitorsStatisticsService;
        private readonly IHttpRequestInfoService _httpRequestInfoService;

        public VisitorsStatisticsController(IVisitorsStatisticsService visitorsStatisticsService, IHttpRequestInfoService httpRequestInfoService)
        {
            _visitorsStatisticsService = visitorsStatisticsService;
            _visitorsStatisticsService.CheckArgumentIsNull(nameof(_visitorsStatisticsService));

            _httpRequestInfoService = httpRequestInfoService;
            _httpRequestInfoService.CheckArgumentIsNull(nameof(_httpRequestInfoService));
        }

        public async Task<IActionResult> Index()
        {
            //var viewModels1 = await _visitorsStatisticsService.GetAllAsync();
            //var viewModels2 = await _visitorsStatisticsService.GetTotalVisitsAsync();
            //var viewModels3 = await _visitorsStatisticsService.GetAllUserOsAsync();
            //var viewModels4 = await _visitorsStatisticsService.GetAllUserBrowserAsync();
            //var viewModels5 = await _visitorsStatisticsService.GetAllPageViewAsync();
            //var viewModels6 = await _visitorsStatisticsService.GetGeneralStatisticsAsync(DateTimeOffset.UtcNow);


            var ip = _httpRequestInfoService.GetIP();
            var userAgent = _httpRequestInfoService.GetHeaderValue("User-Agent");

            var userOs = VisitorsStatisticsHelper.GetUserOsName(userAgent);
            var browserName = VisitorsStatisticsHelper.GetUserBrowserName(userAgent);
            var deviceName = VisitorsStatisticsHelper.GetUserDeviceName(userAgent);

            var viewModel = new CurrentVisitorViewModel()
            {
                Browser = browserName.ToString(),
                BrowserIcon = browserName.Family.ToLowerInvariant(),
                IpAddress = ip,
                CountryName = "",
                OsName = userOs.ToString(),
                OsIcon = userOs.Family.ToLowerInvariant(),
            };

            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("آمار کلی")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RenderGeneralStatisticsAsync()
        {
            var viewModel = await _visitorsStatisticsService.GetGeneralStatisticsAsync(DateTimeOffset.UtcNow);
            var totalVisits = await _visitorsStatisticsService.GetTotalVisitsAsync();
            viewModel.TotalVisits = totalVisits;
            return PartialView("_GeneralStatistics", viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("نمایش بازدیدها به تفکیک مرورگرها")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RenderUserBrowserAsync()
        {
            var viewModels = await _visitorsStatisticsService.GetAllUserBrowserAsync();
            return PartialView("_UserBrowser", viewModels);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("نمایش آمار بازدید به تفکیک سیستم عامل")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RenderUserOsAsync()
        {
            var viewModels = await _visitorsStatisticsService.GetAllUserOsAsync();
            return PartialView("_UserOs", viewModels);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("صفحات مشاهده شده")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RenderPageViewAsync()
        {
            var viewModels = await _visitorsStatisticsService.GetAllPageViewAsync();
            return PartialView("_PageView", viewModels);
        }

    }
}