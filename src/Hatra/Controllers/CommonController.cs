using DNTCommon.Web.Core;
using Hatra.Common.GuardToolkit;
using Hatra.Entities;
using Hatra.Services.Contracts;
using Hatra.Services.Contracts.Identity;
using Hatra.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Hatra.Services.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Hatra.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    public class CommonController : Controller
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly IContactUsService _contactUsService;
        private readonly IFolderService _folderService;
        private readonly IMenuService _menuService;
        private readonly IPageService _pageService;
        private readonly IPictureService _pictureService;
        private readonly ISlideShowService _slideShowService;
        private readonly IStaticContentService _staticContentService;
        private readonly IUsefulLinkService _usefulLinkService;

        private const string RequestNotFound = "اطلاعات درخواستی یافت نشد.";

        public CommonController(
            IApplicationUserManager applicationUserManager,
            IBrandService brandService,
            ICategoryService categoryService,
            IContactUsService contactUsService,
            IFolderService folderService,
            IMenuService menuService,
            IPageService pageService,
            IPictureService pictureService,
            ISlideShowService slideShowService,
            IStaticContentService staticContentService,
            IUsefulLinkService usefulLinkService)
        {
            _userManager = applicationUserManager;
            _userManager.CheckArgumentIsNull(nameof(_userManager));

            _brandService = brandService;
            _brandService.CheckArgumentIsNull(nameof(_brandService));

            _categoryService = categoryService;
            _categoryService.CheckArgumentIsNull(nameof(_categoryService));

            _contactUsService = contactUsService;
            _contactUsService.CheckArgumentIsNull(nameof(_contactUsService));

            _folderService = folderService;
            _folderService.CheckArgumentIsNull(nameof(_folderService));

            _menuService = menuService;
            _menuService.CheckArgumentIsNull(nameof(_menuService));

            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));

            _pictureService = pictureService;
            _pictureService.CheckArgumentIsNull(nameof(_pictureService));

            _slideShowService = slideShowService;
            _slideShowService.CheckArgumentIsNull(nameof(_slideShowService));

            _staticContentService = staticContentService;
            _staticContentService.CheckArgumentIsNull(nameof(_staticContentService));

            _usefulLinkService = usefulLinkService;
            _usefulLinkService.CheckArgumentIsNull(nameof(_usefulLinkService));
        }

        //[AjaxOnly]
        //[DisplayName("Auditable Information")]
        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("Auditable Information")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetAuditableInformation(string typeName, int? id)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                //return PartialView("_AuditableInformation");
                return BadRequest();
            }

            if (id == null)
            {
                //return PartialView("_AuditableInformation");
                return BadRequest();
            }

            AuditableInformationViewModel viewModel;

            switch (typeName)
            {
                case nameof(Brand):
                    viewModel = await _brandService.GetAuditableInformationByIdAsync(id.Value);
                    break;

                case nameof(Category):
                    viewModel = await _categoryService.GetAuditableInformationByIdAsync(id.Value);
                    break;

                case nameof(ContactUs):
                    viewModel = await _contactUsService.GetAuditableInformationByIdAsync(id.Value);
                    break;

                case nameof(Folder):
                    viewModel = await _folderService.GetAuditableInformationByIdAsync(id.Value);
                    break;

                case nameof(Menu):
                    viewModel = await _menuService.GetAuditableInformationByIdAsync(id.Value);
                    break;

                case nameof(Page):
                    viewModel = await _pageService.GetAuditableInformationByIdAsync(id.Value);
                    break;

                case nameof(Picture):
                    viewModel = await _pictureService.GetAuditableInformationByIdAsync(id.Value);
                    break;

                case nameof(SlideShow):
                    viewModel = await _slideShowService.GetAuditableInformationByIdAsync(id.Value);
                    break;

                case nameof(StaticContent):
                    viewModel = await _staticContentService.GetAuditableInformationByIdAsync(id.Value);
                    break;

                case nameof(UsefulLink):
                    viewModel = await _usefulLinkService.GetAuditableInformationByIdAsync(id.Value);
                    break;

                default:
                    //ModelState.AddModelError("", RequestNotFound);
                    //return PartialView("_AuditableInformation");
                    return BadRequest(RequestNotFound);
            }

            if (viewModel == null)
            {
                //ModelState.AddModelError("", RequestNotFound);
                //return PartialView("_AuditableInformation");
                return BadRequest(RequestNotFound);
            }

            //return PartialView("_AuditableInformation", model: viewModel);
            return Json(new { success = true });
        }
    }
}