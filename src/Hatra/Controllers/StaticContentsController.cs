using System;
using System.ComponentModel;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Hatra.Services.Identity;
using Hatra.ViewModels;
using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hatra.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0, GlyphIcon = "fas fa-file-code")]
    [DisplayName("مدیریت محتواهای ثابت")]
    public class StaticContentsController : Controller
    {
        private readonly IStaticContentService _staticContentService;

        private const int DefaultPageSize = 10;
        private const string RequestNotFound = "محتوای ثابت درخواستی یافت نشد.";

        public StaticContentsController(IStaticContentService staticContentService)
        {
            _staticContentService = staticContentService;
            _staticContentService.CheckArgumentIsNull(nameof(_staticContentService));
        }

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index(int? page = 1)
        {
            var model = await _staticContentService.GetAllPagedAsync(page.Value - 1, DefaultPageSize);

            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            return View(model);
        }

        [HttpGet]
        [DisplayName("نمایش فرم محتوای ثابت جدید")]
        [BreadCrumb(Order = 1, GlyphIcon = "fas fa-plus", Title = "محتوای ثابت جدید")]
        public async Task<IActionResult> RenderCreate()
        {
            var nextOrder = await _staticContentService.GetNextOrder();
            var viewModel = new StaticContentViewModel()
            {
                Order = nextOrder,
                IsShow = true,
            };

            return View("Create", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ایجاد یک محتوای ثابت جدید")]
        public async Task<IActionResult> Create(StaticContentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _staticContentService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _staticContentService.InsertAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "StaticContents");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [HttpGet]
        [DisplayName("نمایش فرم ویرایش محتوای ثابت")]
        [BreadCrumb(Order = 1, GlyphIcon = "fas fa-edit")]
        public async Task<IActionResult> RenderEdit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var viewModel = await _staticContentService.GetByIdAsync(id.GetValueOrDefault());

            if (viewModel == null)
            {
                return NotFound();
            }

            this.SetCurrentBreadCrumbTitle($@"ویرایش محتوای ثابت {viewModel.Name}");

            return View("Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ویرایش محتوای ثابت")]
        public async Task<IActionResult> Edit(StaticContentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _staticContentService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _staticContentService.UpdateAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "StaticContents");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [AjaxOnly]
        [DisplayName("نمایش فرم حذف محتوای ثابت")]
        public async Task<IActionResult> RenderDelete([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Delete");
            }

            var viewModel = await _staticContentService.GetByIdAsync(Convert.ToInt32(model.Id));
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
        [DisplayName("حذف محتوای ثابت")]
        public async Task<IActionResult> Delete(StaticContentViewModel viewModel)
        {
            var staticContentViewModel = await _staticContentService.GetByIdAsync(viewModel.Id);
            if (staticContentViewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
            }
            else
            {
                var result = await _staticContentService.DeleteAsync(staticContentViewModel.Id);
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
            var result = await _staticContentService.CheckExistNameAsync(id, name);
            return Json(result ? "نام وارد شده تکراری است" : "true");
        }
    }
}
