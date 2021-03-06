﻿using DNTBreadCrumb.Core;
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
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0, GlyphIcon = "fas fa-tasks")]
    [DisplayName("مدیریت گروه ها")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        private const int DefaultPageSize = 10;
        private const string RequestNotFound = "گروه درخواستی یافت نشد.";

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _categoryService.CheckArgumentIsNull(nameof(_categoryService));
        }

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index(int? page = 1)
        {
            var model = await _categoryService.GetAllPagedAsync(page.Value - 1, DefaultPageSize);

            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            return View(model);
        }

        [HttpGet]
        [DisplayName("نمایش فرم گروه جدید")]
        [BreadCrumb(Order = 1, GlyphIcon = "fas fa-plus", Title = "گروه جدید")]
        public IActionResult RenderCreate()
        {
            var viewModel = new CategoryViewModel()
            {
                IsShow = true,
            };

            return View("Create", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ایجاد یک گروه جدید")]
        public async Task<IActionResult> Create(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _categoryService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _categoryService.InsertAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Categories");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [HttpGet]
        [DisplayName("نمایش فرم ویرایش گروه")]
        [BreadCrumb(Order = 1, GlyphIcon = "fas fa-edit")]
        public async Task<IActionResult> RenderEdit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var viewModel = await _categoryService.GetByIdAsync(id.GetValueOrDefault());

            if (viewModel == null)
            {
                return NotFound();
            }

            this.SetCurrentBreadCrumbTitle($@"ویرایش گروه {viewModel.Name}");

            return View("Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ویرایش گروه")]
        public async Task<IActionResult> Edit(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _categoryService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _categoryService.UpdateAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Categories");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [AjaxOnly]
        [DisplayName("نمایش فرم حذف گروه")]
        public async Task<IActionResult> RenderDelete([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Delete");
            }

            var viewModel = await _categoryService.GetByIdAsync(Convert.ToInt32(model.Id));
            if (viewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
                return PartialView("_Delete");
            }

            if (await _categoryService.CheckExistRelationAsync(viewModel.Id))
            {
                ModelState.AddModelError("", RequestNotFound);
                return PartialView("_Used");
            }

            return PartialView("_Delete", model: viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف گروه")]
        public async Task<IActionResult> Delete(CategoryViewModel viewModel)
        {
            var categoryViewModel = await _categoryService.GetByIdAsync(viewModel.Id);
            if (categoryViewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
            }
            else
            {
                var result = await _categoryService.DeleteAsync(categoryViewModel.Id);
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
            var result = await _categoryService.CheckExistNameAsync(id, name);
            return Json(result ? "نام وارد شده تکراری است" : "true");
        }
    }
}