using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hatra.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("مدیریت منو ها")]
    public class MenusController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IPageService _pageService;
        private readonly ICategoryService _categoryService;

        private const string RequestNotFound = "منو درخواستی یافت نشد.";

        public MenusController(IMenuService menuService, IPageService pageService, ICategoryService categoryService)
        {
            _menuService = menuService;
            _menuService.CheckArgumentIsNull(nameof(_menuService));

            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));

            _categoryService = categoryService;
            _categoryService.CheckArgumentIsNull(nameof(_categoryService));
        }

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var viewModels = await _menuService.GetAllAsync();
            return View(viewModels);
        }

        [HttpGet]
        [DisplayName("نمایش فرم منو جدید")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> RenderCreate()
        {
            var viewModel = new MenuViewModel()
            {
                Link = "#",
                IsShow = true,
            };

            await PopulateMenusAsync(null);
            await PopulateCategoriesAndPagesAsync(null);

            return View("Create", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ایجاد یک منو جدید")]
        public async Task<IActionResult> Create(MenuViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _menuService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    await PopulateMenusAsync(viewModel.ParentId);
                    await PopulateCategoriesAndPagesAsync(viewModel.PageId);
                    return View(viewModel);
                }

                var result = await _menuService.InsertAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Menus");
                }

                await PopulateMenusAsync(viewModel.ParentId);
                await PopulateCategoriesAndPagesAsync(viewModel.PageId);
                return View(viewModel);
            }

            await PopulateMenusAsync(viewModel.ParentId);
            await PopulateCategoriesAndPagesAsync(viewModel.PageId);
            return View(viewModel);
        }

        [HttpGet]
        [DisplayName("نمایش فرم ویرایش منو")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> RenderEdit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var viewModel = await _menuService.GetByIdAsync(id.GetValueOrDefault());

            if (viewModel == null)
            {
                return NotFound();
            }

            await PopulateMenusAsync(viewModel.ParentId);
            await PopulateCategoriesAndPagesAsync(viewModel.PageId);

            return View("Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ویرایش منو")]
        public async Task<IActionResult> Edit(MenuViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _menuService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    await PopulateMenusAsync(viewModel.ParentId);
                    await PopulateCategoriesAndPagesAsync(viewModel.PageId);
                    return View(viewModel);
                }

                var result = await _menuService.UpdateAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Menus");
                }

                await PopulateMenusAsync(viewModel.ParentId);
                await PopulateCategoriesAndPagesAsync(viewModel.PageId);
                return View(viewModel);
            }

            await PopulateMenusAsync(viewModel.ParentId);
            await PopulateCategoriesAndPagesAsync(viewModel.PageId);
            return View(viewModel);
        }

        [AjaxOnly]
        [DisplayName("نمایش فرم حذف منو")]
        public async Task<IActionResult> RenderDelete([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Delete");
            }

            var viewModel = await _menuService.GetByIdAsync(Convert.ToInt32(model.Id));
            if (viewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
                return PartialView("_Delete");
            }

            if (await _menuService.CheckExistRelationAsync(viewModel.Id))
            {
                ModelState.AddModelError("", RequestNotFound);
                return PartialView("_Used");
            }

            return PartialView("_Delete", model: viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف منو")]
        public async Task<IActionResult> Delete(MenuViewModel viewModel)
        {
            var menuViewModel = await _menuService.GetByIdAsync(viewModel.Id);
            if (menuViewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
            }
            else
            {
                var result = await _menuService.DeleteAsync(menuViewModel.Id);
                if (result)
                {
                    return Json(new { success = true });
                }

                ModelState.AddModelError("", RequestNotFound);
            }

            return PartialView("_Delete", model: viewModel);
        }

        private async Task PopulateMenusAsync(int? menuId)
        {
            var data = await _menuService.GetAllParentAsync();

            var selectList = new SelectList(data,
                nameof(DropDownMenuViewModel.Id),
                nameof(DropDownMenuViewModel.Name),
                menuId.GetValueOrDefault());

            ViewBag.PopulateMenus = selectList;
        }

        private async Task PopulateCategoriesAndPagesAsync(int? pageId)
        {
            var data = await _categoryService.GetAllVisibleDropDownMenuAsync();

            data.AddRange(await _pageService.GetAllVisibleWithoutCategoryDropDownMenuAsync());

            var selectList = new SelectList(data,
                nameof(DropDownMenuViewModel.Id),
                nameof(DropDownMenuViewModel.Name),
                pageId.GetValueOrDefault());

            ViewBag.PopulatePages = selectList;
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [DisplayName("اعتبار سنجی نام")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidateName(string name, int id)
        {
            var result = await _menuService.CheckExistNameAsync(id, name);
            return Json(result ? "نام وارد شده تکراری است" : "true");
        }
    }
}