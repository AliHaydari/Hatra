using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hatra.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Hatra.TagHelpers
{
    public class FlagImageFileNameTagHelper : TagHelper
    {
        private readonly string _storageRootPath;
        private const string FLAGS_PATH = @"images\flags";

        private const string ForAttributeName = "asp-for";

        private readonly IHtmlHelper _htmlHelper;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// An expression to be evaluated against the current model
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected string BaseDirectory { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public FlagImageFileNameTagHelper(IHtmlHelper htmlHelper, IHostingEnvironment hostingEnvironment)
        {
            _htmlHelper = htmlHelper;
            _hostingEnvironment = hostingEnvironment;

            //var path = hostingEnvironment.ContentRootPath ?? string.Empty;
            //if (File.Exists(path))
            //    path = Path.GetDirectoryName(path);

            _storageRootPath = Path.Combine(hostingEnvironment.WebRootPath, FLAGS_PATH);

            BaseDirectory = _storageRootPath;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            //clear the output
            output.SuppressOutput();

            var a1 = Directory.EnumerateFiles(BaseDirectory).ToList();
            var a2 = Directory.EnumerateFiles(BaseDirectory, "*.png").ToList();
            var a3 = Directory.EnumerateFiles(BaseDirectory, "*.png",SearchOption.AllDirectories).ToList();

            var flags = Directory.EnumerateFiles(BaseDirectory, "*.png")
                .Select(p => new SelectListItem($@"<span class='image' style='background - image: {p}; width: 16px; height: 11px;'></span><span>{p}</span>", p))
                  .ToList();

            //contextualize IHtmlHelper
            var viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            //get htmlAttributes object
            var htmlAttributes = new Dictionary<string, object>();
            var attributes = context.AllAttributes;
            foreach (var attribute in attributes)
            {
                if (!attribute.Name.Equals(ForAttributeName))
                {
                    htmlAttributes.Add(attribute.Name, attribute.Value);
                }
            }

            //generate editor
            var tagName = For != null ? For.Name : "LanguageCulture";
            if (!string.IsNullOrEmpty(tagName))
            {
                if (htmlAttributes.ContainsKey("class"))
                    htmlAttributes["class"] += " form-control";
                else
                    htmlAttributes.Add("class", "form-control");

                var selectList = _htmlHelper.DropDownList(tagName, flags, htmlAttributes);

                output.Content.SetHtmlContent(selectList.RenderHtmlContent());
            }
        }

        private string GetAbsolutePath(params string[] paths)
        {
            var allPaths = new List<string> { BaseDirectory };
            allPaths.AddRange(paths);

            return Path.Combine(allPaths.ToArray());
        }

        ///// <summary>
        ///// Combines an array of strings into a path
        ///// </summary>
        ///// <param name="paths">An array of parts of the path</param>
        ///// <returns>The combined paths</returns>
        //public virtual string Combine(params string[] paths)
        //{
        //    var path = Path.Combine(paths.SelectMany(p => IsUncPath(p) ? new[] { p } : p.Split('\\', '/')).ToArray());

        //    if (Environment.OSVersion.Platform == PlatformID.Unix && !IsUncPath(path))
        //        //add leading slash to correctly form path in the UNIX system
        //        path = "/" + path;

        //    return path;
        //}

    }
}
