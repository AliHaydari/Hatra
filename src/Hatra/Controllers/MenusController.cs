using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using ExcelDataReader;
using Hatra.Common.Extensions;
using Hatra.Common.GuardToolkit;
using Hatra.Helpers;
using Hatra.Services.Contracts;
using Hatra.Services.Identity;
using Hatra.ViewModels;
using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Hatra.Entities;

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

        private const int DefaultPageSize = 10;
        private const string RequestNotFound = "منو درخواستی یافت نشد.";
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public MenusController(IMenuService menuService, IPageService pageService, ICategoryService categoryService)
        {
            _menuService = menuService;
            _menuService.CheckArgumentIsNull(nameof(_menuService));

            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));

            _categoryService = categoryService;
            _categoryService.CheckArgumentIsNull(nameof(_categoryService));
        }

        public async Task<IActionResult> Export()
        {
            //byte[] reportBytes;
            //using (var package = await createExcelPackage())
            //{
            //    reportBytes = package.GetAsByteArray();
            //}

            //return File(reportBytes, XlsxContentType, "report.xlsx");

            var list = await _menuService.GetAllForExcelExportAsync();
            var excel = ExcelExportHelper.ExportExcel(list, IgnoredColumns: new[] { "SubMenus", "ParentMenu" });
            return File(excel, XlsxContentType, "menu.xlsx");
        }

        [AjaxOnly]
        public IActionResult RenderImport()
        {
            return PartialView("_ImportFromExcel", new ImportFromExcelViewModel());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Import(IFormFile file)
        {
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
                return BadRequest(error: "لطفا فایل اکسل انتخاب کنید");
            }

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

                var list = ExcelExportHelper.ConvertDataTable<Menu>(table);
            }

            using (var memoryStream = new MemoryStream())
            {
                await requestFile.CopyToAsync(memoryStream).ConfigureAwait(false);

                using (var package = new ExcelPackage(memoryStream))
                {
                    try
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        readExcelPackageToString(package, worksheet);

                        return Json(new { success = true });
                    }
                    catch (Exception e)
                    {
                        return BadRequest(error: "خطا در پردازش اطلاعات");
                    }
                }
            }
        }

        [NonAction]
        private async Task<ExcelPackage> createExcelPackage()
        {
            var package = new ExcelPackage();
            package.Workbook.Properties.Title = "Menus Report";
            package.Workbook.Properties.Author = "";
            package.Workbook.Properties.Subject = "Menus Report";
            package.Workbook.Properties.Keywords = "Menus";
            package.Workbook.Properties.Created = DateTime.Now;


            var worksheet = package.Workbook.Worksheets.Add("Menus");

            //First add the headers
            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Link";
            worksheet.Cells[1, 4].Value = "ParentId";
            worksheet.Cells[1, 5].Value = "ParentName";
            worksheet.Cells[1, 6].Value = "Order";
            worksheet.Cells[1, 7].Value = "Type";
            worksheet.Cells[1, 8].Value = "IsShow";
            worksheet.Cells[1, 9].Value = "IsMegaMenu";
            worksheet.Cells[1, 10].Value = "PageId";
            worksheet.Cells[1, 11].Value = "PageSlugUrl";
            worksheet.Cells[1, 12].Value = "CategoryId";
            worksheet.Cells[1, 13].Value = "CategorySlugUrl";

            //Add values

            //var numberformat = "#,##0";
            //var dataCellStyleName = "TableNumber";
            //var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
            //numStyle.Style.Numberformat.Format = numberformat;

            var list = await _menuService.GetAllAsync();
            for (int i = 0; i < list.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = list[i].Id;
                worksheet.Cells[i + 2, 2].Value = list[i].Name;
                worksheet.Cells[i + 2, 3].Value = list[i].Link;
                worksheet.Cells[i + 2, 4].Value = list[i].ParentId;
                worksheet.Cells[i + 2, 5].Value = list[i].ParentName;
                worksheet.Cells[i + 2, 6].Value = list[i].Order;
                worksheet.Cells[i + 2, 7].Value = list[i].Type;
                worksheet.Cells[i + 2, 8].Value = list[i].IsShow;
                worksheet.Cells[i + 2, 9].Value = list[i].IsMegaMenu;
                worksheet.Cells[i + 2, 10].Value = list[i].PageId;
                worksheet.Cells[i + 2, 11].Value = list[i].PageSlugUrl;
                worksheet.Cells[i + 2, 12].Value = list[i].CategoryId;
                worksheet.Cells[i + 2, 13].Value = list[i].CategorySlugUrl;
            }

            //// Add to table / Add summary row
            //var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: 4, toColumn: 4), "Data");
            //tbl.ShowHeader = true;
            //tbl.TableStyle = TableStyles.Dark9;
            //tbl.ShowTotal = true;
            //tbl.Columns[3].DataCellStyleName = dataCellStyleName;
            //tbl.Columns[3].TotalsRowFunction = RowFunctions.Sum;
            //worksheet.Cells[5, 4].Style.Numberformat.Format = numberformat;

            //// AutoFitColumns
            //worksheet.Cells[1, 1, 4, 4].AutoFitColumns();

            //worksheet.HeaderFooter.OddFooter.InsertPicture(
            //    new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, "images", "captcha.jpg")),
            //    PictureAlignment.Right);

            worksheet.Cells.AutoFitColumns();

            return package;
        }

        [NonAction]
        private string readExcelPackageToString(ExcelPackage package, ExcelWorksheet worksheet)
        {
            var rowCount = worksheet.Dimension?.Rows;
            var colCount = worksheet.Dimension?.Columns;

            if (!rowCount.HasValue || !colCount.HasValue)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            for (int row = 1; row <= rowCount.Value; row++)
            {
                for (int col = 1; col <= colCount.Value; col++)
                {
                    sb.AppendFormat("{0}\t", worksheet.Cells[row, col].Value);
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        [DisplayName("ایندکس")]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index(int? page = 1)
        {
            var model = await _menuService.GetAllPagedAsync(page.Value - 1, DefaultPageSize);

            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            return View(model);
        }

        [HttpGet]
        [DisplayName("نمایش فرم منو جدید")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> RenderCreate()
        {
            var nextOrder = await _menuService.GetNextOrder();
            var viewModel = new MenuViewModel()
            {
                Link = "#",
                IsShow = true,
                Order = nextOrder,
            };

            await PopulateMenusAsync(null);
            await PopulatePagesAsync(null);
            await PopulateCategoriesAsync(null);

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
                    await PopulatePagesAsync(viewModel.PageId);
                    await PopulateCategoriesAsync(viewModel.CategoryId);
                    return View(viewModel);
                }

                if (viewModel.CategoryId.HasValue && viewModel.CategoryId != 0)
                {
                    var category = await _categoryService.GetByIdAsync(viewModel.CategoryId.Value);
                    viewModel.CategorySlugUrl = category?.SlugUrl;
                }
                else if (viewModel.PageId.HasValue && viewModel.PageId != 0)
                {
                    var page = await _pageService.GetByIdAsync(viewModel.PageId.Value);
                    viewModel.PageSlugUrl = page?.SlugUrl;
                }

                var result = await _menuService.InsertAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Menus");
                }

                await PopulateMenusAsync(viewModel.ParentId);
                await PopulatePagesAsync(viewModel.PageId);
                await PopulateCategoriesAsync(viewModel.CategoryId);
                return View(viewModel);
            }

            await PopulateMenusAsync(viewModel.ParentId);
            await PopulatePagesAsync(viewModel.PageId);
            await PopulateCategoriesAsync(viewModel.CategoryId);
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
            await PopulatePagesAsync(viewModel.PageId);
            await PopulateCategoriesAsync(viewModel.CategoryId);

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
                    await PopulatePagesAsync(viewModel.PageId);
                    await PopulateCategoriesAsync(viewModel.CategoryId);
                    return View(viewModel);
                }

                if (viewModel.CategoryId.HasValue && viewModel.CategoryId != 0)
                {
                    var category = await _categoryService.GetByIdAsync(viewModel.CategoryId.Value);
                    viewModel.CategorySlugUrl = category?.SlugUrl;
                }
                else if (viewModel.PageId.HasValue && viewModel.PageId != 0)
                {
                    var page = await _pageService.GetByIdAsync(viewModel.PageId.Value);
                    viewModel.PageSlugUrl = page?.SlugUrl;
                }

                var result = await _menuService.UpdateAsync(viewModel);
                if (result)
                {
                    return RedirectToAction("Index", "Menus");
                }

                await PopulateMenusAsync(viewModel.ParentId);
                await PopulatePagesAsync(viewModel.PageId);
                await PopulateCategoriesAsync(viewModel.CategoryId);
                return View(viewModel);
            }

            await PopulateMenusAsync(viewModel.ParentId);
            await PopulatePagesAsync(viewModel.PageId);
            await PopulateCategoriesAsync(viewModel.CategoryId);
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

        [NonAction]
        private async Task PopulateMenusAsync(int? menuId)
        {
            var data = await _menuService.GetAllParentAsync();

            var selectList = new SelectList(data,
                nameof(DropDownMenuViewModel.Id),
                nameof(DropDownMenuViewModel.Name),
                menuId.GetValueOrDefault());

            ViewBag.PopulateMenus = selectList;
        }

        [NonAction]
        private async Task PopulatePagesAsync(int? pageId)
        {
            var data = await _pageService.GetAllVisibleWithoutCategoryDropDownMenuAsync();

            var selectList = new SelectList(data,
                nameof(DropDownMenuViewModel.Id),
                nameof(DropDownMenuViewModel.Name),
                pageId.GetValueOrDefault());

            ViewBag.PopulatePages = selectList;
        }

        [NonAction]
        private async Task PopulateCategoriesAsync(int? categoryId)
        {
            var data = await _categoryService.GetAllVisibleDropDownMenuAsync();

            var selectList = new SelectList(data,
                nameof(DropDownMenuViewModel.Id),
                nameof(DropDownMenuViewModel.Name),
                categoryId.GetValueOrDefault());

            ViewBag.PopulateCategories = selectList;
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