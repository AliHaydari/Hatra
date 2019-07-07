using Hatra.Entities;
using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels
{
    public class StaticContentViewModel
    {
        public StaticContentViewModel()
        {
            
        }

        public StaticContentViewModel(StaticContent staticContent)
        {
            Id = staticContent.Id;
            Name = staticContent.Name;
            Content = staticContent.Content;
            Order = staticContent.Order;
            IsShow = staticContent.IsShow;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "عنوان")]
        [Remote("ValidateName", "StaticContents",
            AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id),
            HttpMethod = "POST")]
        [StringLength(100, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "محتوا")]
        public string Content { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "اولویت")]
        public int Order { get; set; }

        [Display(Name = "نمایش داده شود")]
        public bool IsShow { get; set; }
    }
}
