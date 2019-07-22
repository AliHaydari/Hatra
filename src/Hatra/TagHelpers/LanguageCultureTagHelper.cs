using Hatra.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hatra.TagHelpers
{
    public class LanguageCultureTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        private readonly IHtmlHelper _htmlHelper;

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

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        public LanguageCultureTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
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

            var cultures = System.Globalization.CultureInfo
                .GetCultures(System.Globalization.CultureTypes.SpecificCultures)
                .OrderBy(x => x.EnglishName)
                .Select(x => new SelectListItem
                {
                    Value = x.IetfLanguageTag,
                    Text = $"{x.EnglishName}. {x.IetfLanguageTag}"
                })
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

                var selectList = _htmlHelper.DropDownList(tagName, cultures, htmlAttributes);

                output.Content.SetHtmlContent(selectList.RenderHtmlContent());
            }

        }
    }
}
