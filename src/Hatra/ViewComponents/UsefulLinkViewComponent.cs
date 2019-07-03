using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Hatra.ViewComponents
{
    public class UsefulLinkViewComponent : ViewComponent
    {
        private readonly IUsefulLinkService _usefulLinkService;

        public UsefulLinkViewComponent(IUsefulLinkService usefulLinkService)
        {
            _usefulLinkService = usefulLinkService;
            _usefulLinkService.CheckArgumentIsNull(nameof(_usefulLinkService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModels = await _usefulLinkService.GetAllVisibleAsync();
            return View(viewName: "~/Views/Shared/_UsefulLink.cshtml", viewModels);
        }
    }
}
