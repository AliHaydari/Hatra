using Hatra.Common.WebToolkit;
using Hatra.Services.Contracts;
using Hatra.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Hatra.ViewModels.VisitorsStatistics;

namespace Hatra.Middlewares
{
    public class VisitorCounterMiddleware
    {
        private const string UserAgent = "User-Agent";

        private readonly RequestDelegate _next;

        public VisitorCounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IVisitorsStatisticsService statService)
        {
            string visitorId = context.Request?.Cookies["VisitorId"];
            if (visitorId == null)
            {
                if (!VisitorsStatisticsHelper.IsBotOrCrawler(context.Request?.Headers[UserAgent].ToString()))
                {
                    var userAgent = context.Request?.Headers[UserAgent].ToString();
                    var userIp = context.Connection?.RemoteIpAddress?.ToString();
                    var userOs = VisitorsStatisticsHelper.GetUserOsName(userAgent);
                    var browserName = VisitorsStatisticsHelper.GetUserBrowserName(userAgent);
                    var deviceName = VisitorsStatisticsHelper.GetUserDeviceName(userAgent);
                    var referer = context.Request?.Headers["Referer"].ToString() ?? "Direct";

                    var viewModel = new VisitorsStatisticsViewModel()
                    {
                        UserAgent = userAgent,
                        UserOs = userOs.Family,
                        BrowserName = browserName.Family,
                        DeviceName = deviceName.Family,
                        IpAddress = userIp,
                        PageViewed = context.Request?.Path.Value ?? "/",
                        Referrer = referer,
                        VisitDate = DateTimeOffset.UtcNow,
                    };

                    await statService.InsertAsync(viewModel);

                    context.Response.Cookies.Append("VisitorId", Guid.NewGuid().ToString("N"), new CookieOptions()
                    {
                        Path = "/",
                        HttpOnly = true,
                        Secure = context.Request?.IsHttps ?? false,
                    });
                }
            }

            await _next(context);
        }
    }

    public static class VisitorCounterMiddlewareExtensions
    {
        /// <summary>
        /// آمار بازدید کنندگان سایت
        /// </summary>
        public static IApplicationBuilder UseVisitorsStatistics(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<VisitorCounterMiddleware>();
        }
    }
}
