using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using ExcelDataReader;
using Hatra.Common.Enums;
using Hatra.Common.Extensions;
using Hatra.Common.GuardToolkit;
using Hatra.Common.WebToolkit;
using Hatra.Helpers;
using Hatra.Services.Contracts;
using Hatra.Services.Identity;
using Hatra.ViewModels.Excels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Hatra.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("مدیریت ورودی و خروجی اکسل")]
    public class ExcelExImController : Controller
    {
        private readonly IExcelExImService<ExcelMenuViewModel> _excelExImMenuService;
        private readonly IExcelExImService<ExcelSlideShowViewModel> _excelExImSlideShowService;
        private readonly IExcelExImService<ExcelBrandViewModel> _excelExImBrandService;
        private readonly IExcelExImService<ExcelCategoryViewModel> _excelExImCategoryService;
        private readonly IExcelExImService<ExcelPageViewModel> _excelExImPageService;
        private readonly IExcelExImService<ExcelUsefulLinkViewModel> _excelExImUsefulLinkService;

        public ExcelExImController(
            IExcelExImService<ExcelMenuViewModel> excelExImMenuService,
            IExcelExImService<ExcelSlideShowViewModel> excelExImSlideShowService,
            IExcelExImService<ExcelBrandViewModel> excelExImBrandService,
            IExcelExImService<ExcelCategoryViewModel> excelExImCategoryService,
            IExcelExImService<ExcelPageViewModel> excelExImPageService,
            IExcelExImService<ExcelUsefulLinkViewModel> excelExImUsefulLinkService)
        {
            _excelExImMenuService = excelExImMenuService;
            _excelExImMenuService.CheckArgumentIsNull(nameof(_excelExImMenuService));

            _excelExImSlideShowService = excelExImSlideShowService;
            _excelExImSlideShowService.CheckArgumentIsNull(nameof(_excelExImSlideShowService));

            _excelExImBrandService = excelExImBrandService;
            _excelExImBrandService.CheckArgumentIsNull(nameof(_excelExImBrandService));

            _excelExImCategoryService = excelExImCategoryService;
            _excelExImCategoryService.CheckArgumentIsNull(nameof(_excelExImCategoryService));

            _excelExImPageService = excelExImPageService;
            _excelExImPageService.CheckArgumentIsNull(nameof(_excelExImPageService));

            _excelExImUsefulLinkService = excelExImUsefulLinkService;
            _excelExImUsefulLinkService.CheckArgumentIsNull(nameof(_excelExImUsefulLinkService));
        }

        [Route("export/{type}")]
        [DisplayName("Export")]
        public async Task<IActionResult> Export(ExcelTypeEnum? type)
        {
            if (type == null) return BadRequest();

            byte[] excel = new byte[] { };

            switch (type)
            {
                case ExcelTypeEnum.Menus:
                    excel = await ExportMenusAsync();
                    break;
                case ExcelTypeEnum.SlideShows:
                    excel = await ExportSlideShowsAsync();
                    break;
                case ExcelTypeEnum.Brands:
                    excel = await ExportBrandsAsync();
                    break;
                case ExcelTypeEnum.Categories:
                    excel = await ExportCategoriesAsync();
                    break;
                case ExcelTypeEnum.Pages:
                    excel = await ExportPagesAsync();
                    break;
                case ExcelTypeEnum.UsefulLinks:
                    excel = await ExportUsefulLinksAsync();
                    break;
                default:
                    return NotFound();
            }

            return File(excel, ExcelExportHelper.ExcelContentType, $@"{type.GetValueOrDefault().ToString()}.xlsx");
        }

        //[AjaxOnly]
        //[DisplayName("RenderImport")]
        //public IActionResult RenderImport()
        //{
        //    return PartialView("_ImportFromExcel", new ImportFromExcelViewModel());
        //}

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("import/{type}")]
        [DisplayName("Import")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Import(IFormFile file, ExcelTypeEnum? type)
        {
            if (type == null) return BadRequest();

            if (file == null || file.Length == 0)
            {
                if (HttpContext.Request.Form.Files.Count == 0)
                {
                    return BadRequest(error: "هیچ فایلی انتخاب نشده است");
                }
            }

            var requestFile = file ?? HttpContext.Request.Form.Files[0];

            if (!requestFile.FileName.ContainsExcel())
            {
                return BadRequest(error: "لطفا فایل اکسل (با پسوند .xls یا .xlsx) انتخاب کنید");
            }

            int importResult = 0;
            Stream stre = requestFile.OpenReadStream();
            using (var reader = ExcelReaderFactory.CreateReader(stre))
            {
                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        // true: ردیف اول از فایل را به عنوان هدر در نظر می‌گیرد
                        // مقدار پیش فرض: false
                        UseHeaderRow = true
                    }
                });

                var table = result.Tables[0];

                switch (type)
                {
                    case ExcelTypeEnum.Menus:
                        importResult = await ImportMenusAsync(table);
                        break;
                    case ExcelTypeEnum.SlideShows:
                        importResult = await ImportSlideShowsAsync(table);
                        break;
                    case ExcelTypeEnum.Brands:
                        importResult = await ImportBrandsAsync(table);
                        break;
                    case ExcelTypeEnum.Categories:
                        importResult = await ImportCategoriesAsync(table);
                        break;
                    case ExcelTypeEnum.Pages:
                        importResult = await ImportPagesAsync(table);
                        break;
                    case ExcelTypeEnum.UsefulLinks:
                        importResult = await ImportUsefulLinksAsync(table);
                        break;
                    default:
                        return NotFound();
                }
            }

            if (importResult == -1)
            {
                return BadRequest(error: "خطا در پردازش اطلاعات");
            }

            return Json(new { success = true });

            //using (var memoryStream = new MemoryStream())
            //{
            //    await requestFile.CopyToAsync(memoryStream).ConfigureAwait(false);

            //    using (var package = new ExcelPackage(memoryStream))
            //    {
            //        try
            //        {
            //            var worksheet = package.Workbook.Worksheets[0];
            //            readExcelPackageToString(package, worksheet);

            //            return Json(new { success = true });
            //        }
            //        catch (Exception e)
            //        {
            //            return BadRequest(error: "خطا در پردازش اطلاعات");
            //        }
            //    }
            //}
        }

        #region ExportMethods

        [NonAction]
        private async Task<byte[]> ExportMenusAsync()
        {
            var list = await _excelExImMenuService.ExportToExcelAsync();
            return ExcelExportHelper.ExportExcel(list);
        }

        [NonAction]
        private async Task<byte[]> ExportSlideShowsAsync()
        {
            var list = await _excelExImSlideShowService.ExportToExcelAsync();
            return ExcelExportHelper.ExportExcel(list);
        }

        [NonAction]
        private async Task<byte[]> ExportBrandsAsync()
        {
            var list = await _excelExImBrandService.ExportToExcelAsync();
            return ExcelExportHelper.ExportExcel(list);
        }

        [NonAction]
        private async Task<byte[]> ExportCategoriesAsync()
        {
            var list = await _excelExImCategoryService.ExportToExcelAsync();
            return ExcelExportHelper.ExportExcel(list);
        }

        [NonAction]
        private async Task<byte[]> ExportPagesAsync()
        {
            var list = await _excelExImPageService.ExportToExcelAsync();
            return ExcelExportHelper.ExportExcel(list);
        }

        [NonAction]
        private async Task<byte[]> ExportUsefulLinksAsync()
        {
            var list = await _excelExImUsefulLinkService.ExportToExcelAsync();
            return ExcelExportHelper.ExportExcel(list);
        }

        #endregion

        #region ImportMethods

        [NonAction]
        private async Task<int> ImportMenusAsync(DataTable table)
        {
            var list = ConvertData.ConvertDataTable<ExcelMenuViewModel>(table);
            return await _excelExImMenuService.ImportFromExcelAsync(list);
        }

        [NonAction]
        private async Task<int> ImportSlideShowsAsync(DataTable table)
        {
            var list = ConvertData.ConvertDataTable<ExcelSlideShowViewModel>(table);
            return await _excelExImSlideShowService.ImportFromExcelAsync(list);
        }

        [NonAction]
        private async Task<int> ImportBrandsAsync(DataTable table)
        {
            var list = ConvertData.ConvertDataTable<ExcelBrandViewModel>(table);
            return await _excelExImBrandService.ImportFromExcelAsync(list);
        }

        [NonAction]
        private async Task<int> ImportCategoriesAsync(DataTable table)
        {
            var list = ConvertData.ConvertDataTable<ExcelCategoryViewModel>(table);
            return await _excelExImCategoryService.ImportFromExcelAsync(list);
        }

        [NonAction]
        private async Task<int> ImportPagesAsync(DataTable table)
        {
            var list = ConvertData.ConvertDataTable<ExcelPageViewModel>(table);
            return await _excelExImPageService.ImportFromExcelAsync(list);
        }

        [NonAction]
        private async Task<int> ImportUsefulLinksAsync(DataTable table)
        {
            var list = ConvertData.ConvertDataTable<ExcelUsefulLinkViewModel>(table);
            return await _excelExImUsefulLinkService.ImportFromExcelAsync(list);
        }

        #endregion

    }
}