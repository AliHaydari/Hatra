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
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0, GlyphIcon = "fas fa-copyright")]
    [DisplayName("مدیریت برند ها")]
    public class BrandsController : Controller
    {
        private readonly IBrandService _brandService;

        private const int DefaultPageSize = 10;
        private const string RequestNotFound = "برند درخواستی یافت نشد.";

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
            _brandService.CheckArgumentIsNull(nameof(_brandService));
        }

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index(int? page = 1)
        {
            var model = await _brandService.GetAllPagedAsync(page.Value - 1, DefaultPageSize);

            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            return View(model);
        }

        [HttpGet]
        [DisplayName("نمایش فرم برند جدید")]
        [BreadCrumb(Order = 1, GlyphIcon = "fas fa-plus")]
        public IActionResult RenderCreate()
        {
            var viewModel = new BrandViewModel()
            {
                Link = "#",
                IsShow = true,
            };

            return View("Create", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ایجاد یک برند جدید")]
        public async Task<IActionResult> Create(BrandViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _brandService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _brandService.InsertAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Brands");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [HttpGet]
        [DisplayName("نمایش فرم ویرایش برند")]
        [BreadCrumb(Order = 1, GlyphIcon = "fas fa-edit")]
        public async Task<IActionResult> RenderEdit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var viewModel = await _brandService.GetByIdAsync(id.GetValueOrDefault());

            if (viewModel == null)
            {
                return NotFound();
            }

            return View("Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ویرایش برند")]
        public async Task<IActionResult> Edit(BrandViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _brandService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _brandService.UpdateAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Brands");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [AjaxOnly]
        [DisplayName("نمایش فرم حذف برند")]
        public async Task<IActionResult> RenderDelete([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Delete");
            }

            var viewModel = await _brandService.GetByIdAsync(Convert.ToInt32(model.Id));
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
        [DisplayName("حذف برند")]
        public async Task<IActionResult> Delete(BrandViewModel viewModel)
        {
            var brandViewModel = await _brandService.GetByIdAsync(viewModel.Id);
            if (brandViewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
            }
            else
            {
                var result = await _brandService.DeleteAsync(brandViewModel.Id);
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
            var result = await _brandService.CheckExistNameAsync(id, name);
            return Json(result ? "نام وارد شده تکراری است" : "true");
        }
    }
}
