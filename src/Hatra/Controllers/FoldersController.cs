using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Hatra.Common.GuardToolkit;
using Hatra.FileUpload;
using Hatra.Services.Contracts;
using Hatra.Services.Identity;
using Hatra.ViewModels;
using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hatra.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("مدیریت فولدر ها")]
    public class FoldersController : Controller
    {
        private readonly IFolderService _folderService;
        private readonly IPictureService _pictureService;
        private readonly FilesHelper _filesHelper;
        private readonly FileUploadUtilities _fileUploadUtilities;

        private const string RequestNotFound = "فولدر درخواستی یافت نشد.";
        private const string RequestPictureNotFound = "تصویر درخواستی یافت نشد.";

        public FoldersController(IFolderService folderService, IPictureService pictureService, FilesHelper filesHelper, FileUploadUtilities fileUploadUtilities)
        {
            _folderService = folderService;
            _folderService.CheckArgumentIsNull(nameof(_folderService));

            _pictureService = pictureService;
            _pictureService.CheckArgumentIsNull(nameof(_pictureService));

            _filesHelper = filesHelper;
            _filesHelper.CheckArgumentIsNull(nameof(_filesHelper));

            _fileUploadUtilities = fileUploadUtilities;
            _fileUploadUtilities.CheckArgumentIsNull(nameof(_fileUploadUtilities));
        }

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var viewModels = await _folderService.GetAllAsync();
            return View(viewModels);
        }

        [HttpGet]
        [DisplayName("نمایش فرم فولدر جدید")]
        [BreadCrumb(Order = 1)]
        public IActionResult RenderCreate()
        {
            var viewModel = new FolderViewModel();

            return View("Create", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ایجاد یک فولدر جدید")]
        public async Task<IActionResult> Create(FolderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _folderService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _folderService.InsertAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Folders");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [HttpGet]
        [DisplayName("نمایش فرم ویرایش فولدر")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> RenderEdit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var viewModel = await _folderService.GetByIdAsync(id.GetValueOrDefault());

            if (viewModel == null)
            {
                return NotFound();
            }

            return View("Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ویرایش فولدر")]
        public async Task<IActionResult> Edit(FolderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _folderService.CheckExistNameAsync(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "نام وارد شده تکراری است");
                    return View(viewModel);
                }

                var result = await _folderService.UpdateAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Folders");
                }

                return View(viewModel);
            }

            return View(viewModel);
        }

        [AjaxOnly]
        [DisplayName("نمایش فرم حذف فولدر")]
        public async Task<IActionResult> RenderDelete([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_Delete");
            }

            var viewModel = await _folderService.GetByIdAsync(Convert.ToInt32(model.Id));
            if (viewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
                return PartialView("_Delete");
            }

            if (await _folderService.CheckExistRelationAsync(viewModel.Id))
            {
                ModelState.AddModelError("", RequestNotFound);
                return PartialView("_Used");
            }

            return PartialView("_Delete", model: viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف فولدر")]
        public async Task<IActionResult> Delete(FolderViewModel viewModel)
        {
            var folderViewModel = await _folderService.GetByIdAsync(viewModel.Id);
            if (folderViewModel == null)
            {
                ModelState.AddModelError("", RequestNotFound);
            }
            else
            {
                var result = await _folderService.DeleteAsync(folderViewModel.Id);
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
            var result = await _folderService.CheckExistNameAsync(id, name);
            return Json(result ? "نام وارد شده تکراری است" : "true");
        }

        [HttpGet]
        [DisplayName("نمایش فرم لیست فایل های فولدر")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> RenderPictureList(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var viewModel = await _pictureService.GetAllByFolderIdAsync(id.GetValueOrDefault());

            return View("PictureList", viewModel);
        }

        [AjaxOnly]
        [DisplayName("نمایش فرم حذف فایل")]
        public async Task<IActionResult> RenderDeletePicture([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_DeletePicture");
            }

            var viewModel = await _pictureService.GetByIdAsync(Convert.ToInt32(model.Id));
            if (viewModel == null)
            {
                ModelState.AddModelError("", RequestPictureNotFound);
                return PartialView("_DeletePicture");
            }

            //if (await _folderService.CheckExistRelationAsync(viewModel.Id))
            //{
            //    ModelState.AddModelError("", RequestPictureNotFound);
            //    return PartialView("_Used");
            //}

            return PartialView("_DeletePicture", model: viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف فایل")]
        public async Task<IActionResult> DeletePicture(PictureViewModel viewModel)
        {
            var pictureViewModel = await _pictureService.GetByIdAsync(viewModel.Id);
            if (pictureViewModel == null)
            {
                ModelState.AddModelError("", RequestPictureNotFound);
            }
            else
            {
                var file = await _pictureService.GetByIdAsync(pictureViewModel.Id);
                if (file == null)
                {
                    ModelState.AddModelError("", RequestPictureNotFound);
                    return PartialView("_DeletePicture", model: viewModel);
                }

                var res = _filesHelper.DeleteFile(file.Name);
                if (res == "Ok")
                {
                    var result = await _pictureService.DeleteInTupleAsync(pictureViewModel.Id);
                    if (result.isSuccess)
                    {
                        return Json(new { success = true });
                    }

                    ModelState.AddModelError("", RequestPictureNotFound);
                }

                ModelState.AddModelError("", RequestPictureNotFound);
            }

            return PartialView("_DeletePicture", model: viewModel);
        }

        [HttpGet]
        [DisplayName("نمایش فرم درج فایل")]
        [BreadCrumb(Order = 1)]
        public IActionResult RenderAddPicture(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            //var viewModel = await _pictureService.GetAllByFolderIdAsync(id.GetValueOrDefault());

            var viewModel = new PictureViewModel()
            {
                FolderId = id.GetValueOrDefault(),
            };

            return View("AddPicture", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var result = await _fileUploadUtilities.Handle(HttpContext, HttpContext.Request.Form.Files.ToList(), CancellationToken.None);

            var list = result.FileResults.Select(p => new PictureViewModel(p, id.GetValueOrDefault())).ToList();

            await _pictureService.InsertAllAsync(list);

            var jsonFiles = new JsonFiles(result.FileResults);

            return result.FileResults.Count == 0
                ? Json("Error")
                : Json(jsonFiles);
        }

        [AjaxOnly]
        [DisplayName("نمایش فرم همه فایل ها")]
        public async Task<IActionResult> RenderAllPicture()
        {
            var viewModel = await _pictureService.GetAllAsync();

            return PartialView("_SelectPicture", model: viewModel);
        }

        [AjaxOnly]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [DisplayName("انتخاب تصویر")]
        public async Task<IActionResult> SelectPicture([FromBody]ModelIdViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.Id))
            {
                return PartialView("_SelectPicture");
            }

            var pictureViewModel = await _pictureService.GetByIdAsync(Convert.ToInt32(model.Id));

            return new JsonResult(pictureViewModel);
        }

        public JsonResult GetFileList()
        {
            var list = _filesHelper.GetFileList();
            return Json(list);
        }

        public JsonResult DeleteFile(string file)
        {
            _filesHelper.DeleteFile(file);
            return Json("OK");
        }
    }
}