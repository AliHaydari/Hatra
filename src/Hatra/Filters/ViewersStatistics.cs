using DNTCommon.Web.Core;
using Hatra.Common.GuardToolkit;
using Hatra.Common.WebToolkit;
using Hatra.Services.Contracts;
using Hatra.ViewModels;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using Hatra.ViewModels.VisitorsStatistics;

namespace Hatra.Filters
{
    public class ViewersStatistics : IAsyncActionFilter
    {
        private readonly IVisitorsStatisticsService _statService;
        private readonly IHttpRequestInfoService _httpRequestInfoService;
        private readonly IHtmlHelperService _htmlHelperService;

        public ViewersStatistics(IVisitorsStatisticsService statService, IHttpRequestInfoService httpRequestInfoService, IHtmlHelperService htmlHelperService)
        {
            _statService = statService;
            _statService.CheckArgumentIsNull(nameof(_statService));

            _httpRequestInfoService = httpRequestInfoService;

            _htmlHelperService = htmlHelperService;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            // Do something before the action executes.
            var userAgent = context.HttpContext?.Request?.Headers["User-Agent"].ToString();
            var userIp = context.HttpContext?.Connection?.RemoteIpAddress?.ToString();

            var userOs = VisitorsStatisticsHelper.GetUserOsName(userAgent);
            var browserName = VisitorsStatisticsHelper.GetUserBrowserName(userAgent);
            var deviceName = VisitorsStatisticsHelper.GetUserDeviceName(userAgent);

            //var url = Url.Action("Index", "Home", null, ViewContext.HttpContext.Request.Scheme);

            var getIp = _httpRequestInfoService.GetIP();

            var getHeaderValue = _httpRequestInfoService.GetHeaderValue("Accept-Language");

            var getUserAgent = _httpRequestInfoService.GetUserAgent();

            var absoluteContent = _httpRequestInfoService.AbsoluteContent("~/");
            var absoluteContent2 = _httpRequestInfoService.AbsoluteContent(context.HttpContext?.Request?.Path.Value ?? "~/");

            var getBaseUrl = _httpRequestInfoService.GetBaseUrl();

            var getRawUrl = _httpRequestInfoService.GetRawUrl();

            //var getUrlTitleAsync = await _htmlHelperService.GetUrlTitleAsync(context.HttpContext?.Request?.Path.Value ?? "~/");

            var getUrlHelper = _httpRequestInfoService.GetUrlHelper().Content(context.HttpContext?.Request?.Path.Value ?? "~/");

            var viewModel = new VisitorsStatisticsViewModel()
            {
                UserAgent = userAgent,
                UserOs = userOs.ToString(),
                BrowserName = browserName.ToString(),
                DeviceName = deviceName.ToString(),
                IpAddress = userIp,
                PageViewed = null,
                VisitDate = DateTimeOffset.UtcNow,
            };

            await _statService.InsertAsync(viewModel);

            // next() calls the action method.
            var resultContext = await next();
            // resultContext.Result is set.
            // Do something after the action executes.
        }
    }
}
