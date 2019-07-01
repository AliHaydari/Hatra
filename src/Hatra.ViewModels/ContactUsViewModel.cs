using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Hatra.Entities;

namespace Hatra.ViewModels
{
    public class ContactUsViewModel
    {
        public ContactUsViewModel()
        {

        }

        public ContactUsViewModel(ContactUs contactUs)
        {
            Id = contactUs.Id;
            FullName = contactUs.FullName;
            Email = contactUs.Email;
            Subject = contactUs.Subject;
            Description = contactUs.Description;
            IsAnsered = contactUs.IsAnsered;
            Answer = contactUs.Answer;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام و نام خانوادگی")]
        [StringLength(450, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "ایمیل")]
        [StringLength(450, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        [EmailAddress(ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "موضوع")]
        [StringLength(100, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "پیام")]
        public string Description { get; set; }

        [Display(Name = "پاسخ داده شده")]
        public bool IsAnsered { get; set; }

        [Display(Name = "پاسخ")]
        public string Answer { get; set; }
    }
}
