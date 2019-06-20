using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Hatra.Services.Identity;
using Hatra.ViewModels;
using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Hatra.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("مدیریت صفحه ها")]
    public class PagesController : Controller
    {
        private readonly IPageService _pageService;
        private readonly ICategoryService _categoryService;

        private const int DefaultPageSize = 10;
        private const string RequestNotFound = "صفحه درخواستی یافت نشد.";

        public PagesController(IPageService pageService, ICategoryService categoryService)
        {
            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));

            _categoryService = categoryService;
            _categoryService.CheckArgumentIsNull(nameof(_categoryService));
        }

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index(int? page = 1)
        {
            var model = await _pageService.GetAllPagedAsync(page.Value - 1, DefaultPageSize);

            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            return View(model);
        }

        [HttpGet]
        [DisplayName("نمایش فرم صفحه جدید")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> RenderCreate()
        {
            var viewModel = new PageViewModel()
            {
                IsShow = true,
            };

            await PopulateCategoriesAsync(null);

            return View("Create", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ایجاد یک صفحه جدید")]
        public async Task<IActionResult> Create(PageViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _pageService.CheckExistTitleAsync(viewModel.Id, viewModel.Title))
                {
                    ModelState.AddModelError(nameof(viewModel.Title), "نام وارد شده تکراری است");
                    await PopulateCategoriesAsync(viewModel.CategoryId);
                    return View(viewModel);
                }

                var result = await _pageService.InsertAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Pages");
                }

                await PopulateCategoriesAsync(viewModel.CategoryId);
                return View(viewModel);
            }

            await PopulateCategoriesAsync(viewModel.CategoryId);
            return View(viewModel);
        }

        [HttpGet]
        [DisplayName("نمایش فرم ویرایش صفحه")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> RenderEdit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var viewModel = await _pageService.GetByIdAsync(id.GetValueOrDefault());

            if (viewModel == null)
            {
                return NotFound();
            }

            await PopulateCategoriesAsync(viewModel.CategoryId);

            return View("Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ویرایش صفحه")]
        public async Task<IActionResult> Edit(PageViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _pageService.CheckExistTitleAsync(viewModel.Id, viewModel.Title))
                {
                    ModelState.AddModelError(nameof(viewModel.Title), "نام وارد شده تکراری است");
                    await PopulateCategoriesAsync(viewModel.CategoryId);
                    return View(viewModel);
                }

                var result = await _pageService.UpdateAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Pages");
                }

                await PopulateCategoriesAsync(viewModel.CategoryId);
                return View(viewModel);
            }

            await PopulateCategoriesAsync(viewModel.CategoryId);
            return View(viewModel);
        }

        [AjaxOnly]
        [DisplayName("نمایش فرم حذف صفحه")]
        public async Task<IActionResult> RenderDelete([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Delete");
            }

            var viewModel = await _pageService.GetByIdAsync(Convert.ToInt32(model.Id));
            if (viewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
                return PartialView("_Delete");
            }

            if (await _pageService.CheckExistRelationAsync(viewModel.Id))
            {
                ModelState.AddModelError("", RequestNotFound);
                return PartialView("_Used");
            }

            return PartialView("_Delete", model: viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف صفحه")]
        public async Task<IActionResult> Delete(PageViewModel viewModel)
        {
            var menuViewModel = await _pageService.GetByIdAsync(viewModel.Id);
            if (menuViewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
            }
            else
            {
                var result = await _pageService.DeleteAsync(menuViewModel.Id);
                if (result)
                {
                    return Json(new { success = true });
                }

                ModelState.AddModelError("", RequestNotFound);
            }

            return PartialView("_Delete", model: viewModel);
        }

        [NonAction]
        private async Task PopulateCategoriesAsync(int? categoryId)
        {
            var data = await _categoryService.GetAllAsync();

            var selectList = new SelectList(data,
                nameof(CategoryViewModel.Id),
                nameof(CategoryViewModel.Name),
                categoryId.GetValueOrDefault());

            ViewBag.PopulateCategories = selectList;
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [DisplayName("اعتبار سنجی عنوان")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidateTitle(string title, int id)
        {
            var result = await _pageService.CheckExistTitleAsync(id, title);
            return Json(result ? "نام وارد شده تکراری است" : "true");
        }
    }
}