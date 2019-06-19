using Hatra.Entities;
using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Hatra.ViewModels
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
            PageViewModels = new List<PageViewModel>();
        }

        public CategoryViewModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
            IsShow = category.IsShow;
            SlugUrl = category.SlugUrl;

            PageViewModels = category.Pages?.Select(p => new PageViewModel(p)).ToList() ?? new List<PageViewModel>();
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام")]
        [Remote("ValidateName", "Categories",
            AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id),
            HttpMethod = "POST")]
        [StringLength(450, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "نمایش داده شود")]
        public bool IsShow { get; set; }

        [Display(Name = "SEO-Url")]
        public string SlugUrl { get; set; }

        public List<PageViewModel> PageViewModels { get; set; }
    }
}
