using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hatra.Common.GuardToolkit;
using Hatra.FileUpload;
using Hatra.Services.Contracts;
using Hatra.ViewModels.FileUpload;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hatra.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly FilesHelper _filesHelper;
        private readonly FileUploadUtilities _fileUploadUtilities;
        private readonly IPictureService _pictureService;

        public FileUploadController(FilesHelper filesHelper, FileUploadUtilities fileUploadUtilities, IPictureService pictureService)
        {
            _filesHelper = filesHelper;
            _filesHelper.CheckArgumentIsNull(nameof(_filesHelper));

            _fileUploadUtilities = fileUploadUtilities;
            _fileUploadUtilities.CheckArgumentIsNull(nameof(_fileUploadUtilities));

            _pictureService = pictureService;
            _pictureService.CheckArgumentIsNull(nameof(_pictureService));
        }

        public ActionResult Index()
        {
            JsonFiles listOfFiles = _filesHelper.GetFileList();
            var model = new FilesViewModel()
            {
                Files = listOfFiles.files
            };

            return View("Index2", model);
        }

        public ActionResult Show()
        {
            JsonFiles listOfFiles = _filesHelper.GetFileList();
            var model = new FilesViewModel()
            {
                Files = listOfFiles.files
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(List<IFormFile> files)
        {
            var result = await _fileUploadUtilities.Handle(HttpContext, files, CancellationToken.None);

            var jsonFiles = new JsonFiles(result.FileResults);

            return result.FileResults.Count == 0
                ? Json("Error")
                : Json(jsonFiles);
        }

        public JsonResult GetFileList()
        {
            var list = _filesHelper.GetFileList();
            return Json(list);
        }

        public async Task<JsonResult> DeleteFile(string file)
        {
            var pictureViewModel = await _pictureService.GetByNameAsync(file);
            if (pictureViewModel == null)
            {
                return Json("Error");
            }
            else
            {
                var res = _filesHelper.DeleteFile(file);
                if (res == "Ok")
                {
                    var result = await _pictureService.DeleteAsync(pictureViewModel.Id);
                    if (result)
                    {
                        return Json("OK");
                    }
                }

                return Json("Error");
            }
        }
    }
}