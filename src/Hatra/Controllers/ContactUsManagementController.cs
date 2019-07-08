using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using DNTPersianUtils.Core;
using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Hatra.Services.Contracts.Identity;
using Hatra.Services.Identity;
using Hatra.ViewModels;
using Hatra.ViewModels.Identity;
using Hatra.ViewModels.Identity.Emails;
using Hatra.ViewModels.Identity.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Hatra.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("مدیریت تماس مشتری ها")]
    public class ContactUsManagementController : Controller
    {
        private readonly IContactUsService _contactUsService;
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
        private readonly IEmailSender _emailSender;

        private const int DefaultPageSize = 10;
        private const string RequestNotFound = "تماس مشتری درخواستی یافت نشد.";

        public ContactUsManagementController(IContactUsService contactUsService, IOptionsSnapshot<SiteSettings> siteOptions, IEmailSender emailSender)
        {
            _contactUsService = contactUsService;
            _contactUsService.CheckArgumentIsNull(nameof(_contactUsService));

            _siteOptions = siteOptions;
            _siteOptions.CheckArgumentIsNull(nameof(_siteOptions));

            _emailSender = emailSender;
            _emailSender.CheckArgumentIsNull(nameof(_emailSender));
        }

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index(int? page = 1)
        {
            var model = await _contactUsService.GetAllPagedAsync(page.Value - 1, DefaultPageSize);

            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            return View(model);
        }

        [HttpGet]
        [DisplayName("نمایش فرم پاسخ تماس مشتری")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> RenderEdit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var viewModel = await _contactUsService.GetByIdAsync(id.GetValueOrDefault());

            if (viewModel == null)
            {
                return NotFound();
            }

            return View("Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("پاسخ تماس مشتری")]
        public async Task<IActionResult> Edit(ContactUsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(viewModel.Answer))
                {
                    ModelState.AddModelError(nameof(viewModel.Answer), "لطفا متن پاسخ را وارد کنید");
                    return View(viewModel);
                }

                var result = await _contactUsService.UpdateAsync(viewModel);
                if (result)
                {
                    await _emailSender.SendEmailAsync(
                        email: viewModel.Email,
                        subject: "پاسخ پیام",
                        viewNameOrPath: "~/Areas/Identity/Views/EmailTemplates/_ContactUsAnswer.cshtml",
                        model: new ContactUsConfirmationViewModel
                        {
                            Id = viewModel.Id,
                            FullName = viewModel.FullName,
                            Email = viewModel.Email,
                            Subject = viewModel.Subject,
                            Description = viewModel.Description,
                            Answer = viewModel.Answer,
                            EmailSignature = _siteOptions.Value.Smtp.FromName,
                            MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                        });

                    return RedirectToAction("Index", "ContactUsManagement");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{id:int?}")]
        [DisplayName("باز ارسال پاسخ تماس مشتری")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ReSend(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var viewModel = await _contactUsService.GetByIdAsync(id.Value);

            if (viewModel == null) return BadRequest();

            await _emailSender.SendEmailAsync(
                email: viewModel.Email,
                subject: "پاسخ پیام",
                viewNameOrPath: "~/Areas/Identity/Views/EmailTemplates/_ContactUsAnswer.cshtml",
                model: new ContactUsConfirmationViewModel
                {
                    Id = viewModel.Id,
                    FullName = viewModel.FullName,
                    Email = viewModel.Email,
                    Subject = viewModel.Subject,
                    Description = viewModel.Description,
                    Answer = viewModel.Answer,
                    EmailSignature = _siteOptions.Value.Smtp.FromName,
                    MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                });

            return Json(new { success = true });
        }

        [AjaxOnly]
        [DisplayName("نمایش فرم حذف تماس مشتری")]
        public async Task<IActionResult> RenderDelete([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Delete");
            }

            var viewModel = await _contactUsService.GetByIdAsync(Convert.ToInt32(model.Id));
            if (viewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
                return PartialView("_Delete");
            }

            return PartialView("_Delete", model: viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف تماس مشتری")]
        public async Task<IActionResult> Delete(ContactUsViewModel viewModel)
        {
            var contactUsViewModel = await _contactUsService.GetByIdAsync(viewModel.Id);
            if (contactUsViewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
            }
            else
            {
                var result = await _contactUsService.DeleteAsync(contactUsViewModel.Id);
                if (result)
                {
                    return Json(new { success = true });
                }

                ModelState.AddModelError("", RequestNotFound);
            }

            return PartialView("_Delete", model: viewModel);
        }
    }
}