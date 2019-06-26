using Hatra.Entities;
using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels
{
    public class BrandViewModel
    {
        public BrandViewModel()
        {

        }

        public BrandViewModel(Brand brand)
        {
            Id = brand.Id;
            Name = brand.Name;
            Image = brand.Image;
            Link = brand.Link;
            Description = brand.Description;
            IsShow = brand.IsShow;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام")]
        [Remote("ValidateName", "Brands",
            AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id),
            HttpMethod = "POST")]
        [StringLength(100, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "تصویر")]
        public string Image { get; set; }

        [Display(Name = "آدرس")]
        public string Link { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "نمایش داده شود")]
        public bool IsShow { get; set; }
    }
}
