using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Hatra.Services.Identity;
using Hatra.ViewModels;
using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Hatra.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0, GlyphIcon = "fa fa-link")]
    [DisplayName("مدیریت لینک های مفید")]
    public class UsefulLinksController : Controller
    {
        private readonly IUsefulLinkService _usefulLinkService;

        private const int DefaultPageSize = 10;
        private const string RequestNotFound = "لینک مفید درخواستی یافت نشد.";

        public UsefulLinksController(IUsefulLinkService usefulLinkService)
        {
            _usefulLinkService = usefulLinkService;
            _usefulLinkService.CheckArgumentIsNull(nameof(_usefulLinkService));
        }

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index(int? page = 1)
        {
            var model = await _usefulLinkService.GetAllPagedAsync(page.Value - 1, DefaultPageSize);

            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            return View(model);
        }

        [HttpGet]
        [DisplayName("نمایش فرم لینک مفید جدید")]
        [BreadCrumb(Order = 1, GlyphIcon = "fas fa-plus", Title = "لینک مفید جدید")]
        public async Task<IActionResult> RenderCreate()
        {
            var nextOrder = await _usefulLinkService.GetNextOrder();
            var viewModel = new UsefulLinkViewModel()
            {
                Link = "#",
                Order = nextOrder,
                IsShow = true,
            };

            return View("Create", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ایجاد یک لینک مفید جدید")]
        public async Task<IActionResult> Create(UsefulLinkViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _usefulLinkService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _usefulLinkService.InsertAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "UsefulLinks");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [HttpGet]
        [DisplayName("نمایش فرم ویرایش لینک مفید")]
        [BreadCrumb(Order = 1, GlyphIcon = "fas fa-edit")]
        public async Task<IActionResult> RenderEdit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var viewModel = await _usefulLinkService.GetByIdAsync(id.GetValueOrDefault());

            if (viewModel == null)
            {
                return NotFound();
            }

            this.SetCurrentBreadCrumbTitle($@"ویرایش لینک مفید {viewModel.Name}");

            return View("Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ویرایش لینک مفید")]
        public async Task<IActionResult> Edit(UsefulLinkViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _usefulLinkService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _usefulLinkService.UpdateAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "UsefulLinks");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [AjaxOnly]
        [DisplayName("نمایش فرم حذف لینک مفید")]
        public async Task<IActionResult> RenderDelete([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Delete");
            }

            var viewModel = await _usefulLinkService.GetByIdAsync(Convert.ToInt32(model.Id));
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
        [DisplayName("حذف لینک مفید")]
        public async Task<IActionResult> Delete(UsefulLinkViewModel viewModel)
        {
            var usefulLinkViewModel = await _usefulLinkService.GetByIdAsync(viewModel.Id);
            if (usefulLinkViewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
            }
            else
            {
                var result = await _usefulLinkService.DeleteAsync(usefulLinkViewModel.Id);
                if (result)
                {
                    return Json(new { success = true });
                }

                ModelState.AddModelError("", RequestNotFound);
            }

            return PartialView("_Delete", model: viewModel);
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [DisplayName("اعتبار سنجی نام")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidateName(string name, int id)
        {
            var result = await _usefulLinkService.CheckExistNameAsync(id, name);
            return Json(result ? "نام وارد شده تکراری است" : "true");
        }
    }
}
