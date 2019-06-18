using Hatra.Entities;
using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels
{
    public class SlideShowViewModel
    {
        public SlideShowViewModel()
        {

        }

        public SlideShowViewModel(SlideShow slideShow)
        {
            Id = slideShow.Id;
            Title = slideShow.Title;
            BriefDescription = slideShow.BriefDescription;
            Description = slideShow.Description;
            Image = slideShow.Image;
            Link1 = slideShow.Link1;
            Link2 = slideShow.Link2;
            Order = slideShow.Order;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "عنوان")]
        [Remote("ValidateTitle", "SlideShow",
            AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id),
            HttpMethod = "POST")]
        [StringLength(50, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Title { get; set; }

        [Display(Name = "توضیحات مختصر")]
        [StringLength(50, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string BriefDescription { get; set; }

        [Display(Name = "متن")]
        //[MaxLength(2000, ErrorMessage = "{0} باید حداکثر {1} حرف باشد")]
        public string Description { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "تصویر")]
        public string Image { get; set; }

        [Display(Name = "لینک یک")]
        public string Link1 { get; set; }

        [Display(Name = "لینک دو")]
        public string Link2 { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "اولویت")]
        public int Order { get; set; }
    }
}
