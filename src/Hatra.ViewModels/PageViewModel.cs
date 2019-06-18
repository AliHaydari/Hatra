using Hatra.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Hatra.ViewModels.Identity;

namespace Hatra.ViewModels
{
    public class PageViewModel
    {
        public PageViewModel()
        {

        }

        public PageViewModel(Page page)
        {
            Id = page.Id;
            Title = page.Title;
            BriefDescription = page.BriefDescription;
            Body = page.Body;
            MetaDescription = page.MetaDescription;
            SlugUrl = page.SlugUrl;
            ViewNumber = page.ViewNumber;
            Image = page.Image;
            Order = page.Order;
            CategoryId = page.CategoryId;
            CategoryName = page.Category?.Name;
            IsShow = page.IsShow;
            PageImageViewModels = page.Images?.Select(p => new PageImageViewModel(p)).ToList();
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "عنوان")]
        [Remote("ValidateTitle", "Pages",
            AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id),
            HttpMethod = "POST")]
        [StringLength(500, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Title { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "توضیحات مختصر")]
        [StringLength(500, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string BriefDescription { get; set; }

        [Display(Name = "متن")]
        public string Body { get; set; }

        [Display(Name = "SEO-Meta")]
        public string MetaDescription { get; set; }

        [Display(Name = "SEO-Url")]
        public string SlugUrl { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int ViewNumber { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "اولویت")]
        public int Order { get; set; }

        [Display(Name = "گروه")]
        public int? CategoryId { get; set; }

        [Display(Name = "گروه")]
        public string CategoryName { get; set; }

        [Display(Name = "نمایش داده شود")]
        public bool IsShow { get; set; }

        public string CreateDateTime { get; set; }

        public List<PageImageViewModel> PageImageViewModels { get; set; }
    }
}
