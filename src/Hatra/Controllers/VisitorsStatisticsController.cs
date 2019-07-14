using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Hatra.Common.GuardToolkit;
using Hatra.Common.WebToolkit;
using Hatra.Services.Contracts;
using Hatra.Services.Identity;
using Hatra.ViewModels.VisitorsStatistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

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

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var totalVisits = await _visitorsStatisticsService.GetTotalVisitsAsync();

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
                TotalVisits = totalVisits,
            };

            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [DisplayName("آمار کلی")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RenderGeneralStatisticsAsync(long? totalVisits)
        {
            var viewModel = await _visitorsStatisticsService.GetGeneralStatisticsAsync(DateTimeOffset.UtcNow);
            viewModel.TotalVisits = totalVisits ?? await _visitorsStatisticsService.GetTotalVisitsAsync();
            return PartialView("_GeneralStatistics", viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [DisplayName("نمایش بازدیدها به تفکیک مرورگرها")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RenderUserBrowserAsync(long? totalVisits)
        {
            var viewModels = await _visitorsStatisticsService.GetAllUserBrowserAsync();

            var viewModel = new StatisticsHelperViewModel<UserBrowserViewModel>()
            {
                TotalVisits = totalVisits ?? await _visitorsStatisticsService.GetTotalVisitsAsync(),
                ViewModels = viewModels,
            };

            return PartialView("_UserBrowser", viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [DisplayName("نمایش آمار بازدید به تفکیک سیستم عامل")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RenderUserOsAsync(long? totalVisits)
        {
            var viewModels = await _visitorsStatisticsService.GetAllUserOsAsync();

            var viewModel = new StatisticsHelperViewModel<UserOsViewModel>()
            {
                TotalVisits = totalVisits ?? await _visitorsStatisticsService.GetTotalVisitsAsync(),
                ViewModels = viewModels,
            };

            return PartialView("_UserOs", viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [DisplayName("صفحات مشاهده شده")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RenderPageViewAsync(long? totalVisits)
        {
            var viewModels = await _visitorsStatisticsService.GetAllPageViewAsync();
            return PartialView("_PageView", viewModels);
        }

        [AjaxOnly]
        [HttpPost]
        [DisplayName("ارجاعات")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RenderReferrerAsync(long? totalVisits)
        {
            var viewModels = await _visitorsStatisticsService.GetAllReferrerAsync();
            return PartialView("_Referrer", viewModels);
        }

        [AjaxOnly]
        [HttpPost]
        [DisplayName("آمار بازدید در تاریخ انتخابی")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RenderRangeDateAsync([FromBody]RangeDateSelectedViewModel model)
        {
            //if (!model.FromDate.HasValue)
            //    return BadRequest("لطفا تاریخ شروع را انتخاب کنید");

            //if (!model.ToDate.HasValue)
            //    return BadRequest("لطفا تاریخ پایان را انتخاب کنید");

            if (!model.FromDate.HasValue)
                model.FromDate = DateTimeOffset.MinValue;

            if (!model.ToDate.HasValue)
                model.ToDate = DateTimeOffset.MaxValue;

            var viewModel = await _visitorsStatisticsService.GetInRangeDateAsync(model.FromDate.Value, model.ToDate.Value);
            return PartialView("_InRangeDate", viewModel);
        }

    }
}