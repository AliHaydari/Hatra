﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Hatra.Common.GuardToolkit;
using Hatra.Services.Identity;
using Hatra.ViewModels.Identity.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Hatra.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [DisplayName("مدیریت تنظیمات نمایشی سایت")]
    public class SiteSettingsController : Controller
    {
        private readonly IOptionsSnapshot<ShowingSettingSite> _settings;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SiteSettingsController(IOptionsSnapshot<ShowingSettingSite> settings, IHostingEnvironment hostingEnvironment)
        {
            _settings = settings;
            _settings.CheckArgumentIsNull(nameof(_settings));

            _hostingEnvironment = hostingEnvironment;
            _hostingEnvironment.CheckArgumentIsNull(nameof(_hostingEnvironment));
        }

        [HttpGet]
        [DisplayName("نمایش فرم تنظیمات نمایشی سایت")]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> Index()
        {
            var path = Path.Combine(_hostingEnvironment.ContentRootPath, "ShowingSettingSite.json");

            //if (!System.IO.File.Exists(path))
            //{
            //    System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(new ShowingSettingSite()));
            //}

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var showingSettingSite = JsonConvert.DeserializeObject<ShowingSettingSite>(await System.IO.File.ReadAllTextAsync(path, Encoding.UTF8));

            return View(showingSettingSite);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [DisplayName("ثبت تنظیمات نمایشی سایت")]
        public IActionResult Save(ShowingSettingSite viewModel)
        {
            var path = Path.Combine(_hostingEnvironment.ContentRootPath, "ShowingSettingSite.json");
            System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(viewModel));

            return View("Index");
        }
    }
}