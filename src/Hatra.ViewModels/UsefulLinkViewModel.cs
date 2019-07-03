using Hatra.Entities;
using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels
{
    public class UsefulLinkViewModel
    {
        public UsefulLinkViewModel()
        {

        }

        public UsefulLinkViewModel(UsefulLink usefulLink)
        {
            Id = usefulLink.Id;
            Name = usefulLink.Name;
            Link = usefulLink.Link;
            Order = usefulLink.Order;
            IsShow = usefulLink.IsShow;
            Description = usefulLink.Description;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام")]
        [Remote("ValidateName", "UsefulLinks",
            AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id),
            HttpMethod = "POST")]
        [StringLength(100, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "لینک")]
        public string Link { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "اولویت")]
        public int Order { get; set; }

        [Display(Name = "نمایش داده شود")]
        public bool IsShow { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }
    }
}
