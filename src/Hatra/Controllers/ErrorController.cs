using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using Hatra.ViewModels;
using System.Net;

namespace Hatra.Controllers
{
    [BreadCrumb(Title = "خطا", UseDefaultRouteUrl = true, Order = 0, GlyphIcon = "fas fa-warning")]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// More info: http://www.dotnettips.info/post/2446
        /// </summary>
        [BreadCrumb(Title = "ایندکس", Order = 2, GlyphIcon = "fas fa-navicon")]
        public IActionResult Index(int? id)
        {
            var logBuilder = new StringBuilder();

            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            logBuilder.AppendLine($"Error {id} for {Request.Method} {statusCodeReExecuteFeature?.OriginalPath ?? Request.Path.Value}{Request.QueryString.Value}\n");

            var exceptionHandlerFeature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlerFeature?.Error != null)
            {
                var exception = exceptionHandlerFeature.Error;
                logBuilder.AppendLine($"<h1>Exception: {exception.Message}</h1>{exception.StackTrace}");
            }

            foreach (var header in Request.Headers)
            {
                var headerValues = string.Join(",", value: header.Value);
                logBuilder.AppendLine($"{header.Key}: {headerValues}");
            }
            _logger.LogError(logBuilder.ToString());

            var viewModel = new ErrorViewModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated
            };

            if (id == null)
            {
                viewModel.Title = "خطا!";
                viewModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                viewModel.Description = "متاسفانه در حین پردازش درخواست جاری خطایی رخ داده‌است.";

                //return View("Error");
                return View("ErrorN", viewModel);
            }

            //switch (id.Value)
            //{
            //    case 401:
            //    case 403:
            //        return View("AccessDenied");
            //    case 404:
            //        return View("NotFound");

            //    default:
            //        return View("Error");
            //}
            switch (id.Value)
            {
                case 401:
                case 403:
                    viewModel.Title = "عدم دسترسی!";
                    viewModel.StatusCode = (int)HttpStatusCode.Unauthorized;
                    viewModel.Description = "متاسفانه شما مجوز دسترسی به صفحه‌ی درخواستی را ندارید.";
                    return View("ErrorN", viewModel);
                case 404:
                    viewModel.Title = "یافت نشد!";
                    viewModel.StatusCode = (int)HttpStatusCode.NotFound;
                    viewModel.Description = "اطلاعات درخواستی یافت نشد.";
                    return View("ErrorN", viewModel);
                default:
                    viewModel.Title = "خطا!";
                    viewModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                    viewModel.Description = "متاسفانه در حین پردازش درخواست جاری خطایی رخ داده‌است.";
                    return View("ErrorN", viewModel);
            }

        }
    }
}