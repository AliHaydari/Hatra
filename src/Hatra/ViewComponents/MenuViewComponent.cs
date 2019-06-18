using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Hatra.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;

        public MenuViewComponent(IMenuService menuService)
        {
            _menuService = menuService;
            _menuService.CheckArgumentIsNull(nameof(_menuService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModels = await _menuService.GetAllAsync();
            return View(viewName: "~/Views/Shared/_Menu.cshtml", viewModels.Where(p => p.IsShow).ToList());
        }
    }
}
