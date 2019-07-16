using System;
using System.Collections.Generic;
using System.ComponentModel;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;
using DNTCommon.Web.Core;
using Hatra.ViewModels;

namespace Hatra.Areas.Identity.Controllers
{
    [Area(AreaConstants.IdentityArea)]
    [Authorize]
    [BreadCrumb(Title = "خانه", UseDefaultRouteUrl = true, Order = 0)]
    public class HomeController : Controller
    {
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {
            //var sss = System.AppContext.BaseDirectory;

            //var ssss = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

            //var list = new List<DriveInfoViewModel>();

            //DriveInfo[] allDrives = DriveInfo.GetDrives();

            //foreach (var drive in allDrives)
            //{
            //    try
            //    {
            //        list.Add(new DriveInfoViewModel()
            //        {
            //            Name = drive.Name,
            //            DriveType = drive.DriveType.ToString(),
            //            VolumeLabel = drive.VolumeLabel,
            //            DriveFormat = drive.DriveFormat,
            //            AvailableFreeSpace = drive.AvailableFreeSpace,
            //            TotalFreeSpace = drive.TotalFreeSpace,
            //            TotalSize = drive.TotalSize,
            //        });
            //    }
            //    catch
            //    {
            //        list.Add(new DriveInfoViewModel()
            //        {
            //            Name = "",
            //            DriveType = "",
            //            VolumeLabel = "",
            //            DriveFormat = "",
            //            AvailableFreeSpace = 0,
            //            TotalFreeSpace = 0,
            //            TotalSize = 0,
            //        });
            //    }


            //var dname = $@"Drive {drive.Name}";
            //var dType = $@"Drive type: {drive.DriveType}";

            //if (drive.IsReady == true)
            //{
            //    var a1 = $@"Volume label: {drive.VolumeLabel}";
            //    var a2 = $@"File system: {drive.DriveFormat}";
            //    var a3 = $@"Available space to current user: {drive.AvailableFreeSpace,15} bytes";
            //    var a4 = $@"Total available space: {drive.TotalFreeSpace,15} bytes";
            //    var a5 = $@"Total size of drive: {drive.TotalSize,15} bytes ";

            //    var afs1 = ((drive.AvailableFreeSpace / 1024) / 1024); //MB
            //}
            //}

            return View();
        }

        [AjaxOnly]
        [HttpPost]
        [DisplayName("Copy To Clipboard")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult CopyToClipboard(string text)
        {
            TextCopy.Clipboard.SetText(text);
            return Json(new { success = true, txt = text });
        }

        [AjaxOnly]
        [HttpGet]
        [DisplayName("Paste From Clipboard")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult PasteFromClipboard()
        {
            var text = TextCopy.Clipboard.GetText();
            return Json(new { success = true, txt = text });
        }
    }
}