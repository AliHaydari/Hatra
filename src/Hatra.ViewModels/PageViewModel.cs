using System;
using Hatra.Entities;
using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DNTPersianUtils.Core;
using Hatra.Common.Constants;
using Hatra.Common.WebToolkit;

namespace Hatra.ViewModels
{
    public class PageViewModel
    {
        public PageViewModel()
        {
            PageImageViewModels = new List<PageImageViewModel>();
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
            IsShowInLastContent = page.IsShowInLastContent;

            PageImageViewModels = page.Images?.Select(p => new PageImageViewModel(p)).ToList() ?? new List<PageImageViewModel>();
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

        public string CategorySlugUrl { get; set; }

        [Display(Name = "نمایش داده شود")]
        public bool IsShow { get; set; }

        [Display(Name = "نمایش در آخرین مطالب")]
        public bool IsShowInLastContent { get; set; }

        public List<PageImageViewModel> PageImageViewModels { get; set; }


        public string ImageName => Image?.Length >= 53 ? Image?.Remove(0, 21).Substring(0, 32) : "";
        public string ImageExtension => Image?.Length >= 53 ? Image?.Remove(0, 21).Remove(0, 33) : "";
        public string ImageThumbnail => ImageName + $@"{ImageConstants.Thumb370X180}." + ImageExtension;
        public string ImageThumbnailPath => "/UploadedFiles/Files/thumbs/" + ImageThumbnail;
        public string CreatedPersianDateTime => CreatedDateTime.ToLongPersianDateString().ToPersianNumbers();

        public int CreatedByUserId { get; set; }
        public string CreatedUserName { get; set; }
        public string CreatedUserNameSlugUrl => string.IsNullOrWhiteSpace(CreatedUserName) ? "" : SeoHelpers.GenerateSlug(CreatedUserName);

        public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset ModifiedDateTime { get; set; }

        public DateTime CreatedDateTimeInDateTime => CreatedDateTime.UtcDateTime;
        public DateTime ModifiedDateTimeInDateTime => ModifiedDateTime.UtcDateTime;
    }
}
