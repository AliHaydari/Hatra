using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels.Identity.Settings
{
    public class ShowingSettingSite
    {
        public ShowingSettingSite()
        {
            EnglishSiteName = "EnglishSiteName";
            PersianSiteName = "نام فارسی سایت";
            Description = "A short description of the site";
            FooterDescription = "A short description of the footer site";
            SiteKeywords = "";
            Owner = "The Owner";
            Email = "site@email.com";
            SiteUrl = "localhost";
            WorkTime = "شنبه تا پنج شنبه:9 تا 17";
            Tell1 = "031-33333333";
            Tell2 = "031-33333333";
            Address = "Address";
            Twitter = "Twitter Account Address";
            Facebook = "Facebook Account Address";
            Skype = "Skype Account Address";
            Pinterest = "Pinterest Account Address";
            Telegram = "Telegram Account Address";
            Instagram = "Instagram Account Address";
        }

        [Display(Name = "نام انگلیسی سایت")]
        public string EnglishSiteName { get; set; }

        [Display(Name = "نام فارسی سایت")]
        public string PersianSiteName { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "توضیحات پایین صفحه")]
        public string FooterDescription { get; set; }

        [Display(Name = "کلید واژه ها")]
        public string SiteKeywords { get; set; }

        [Display(Name = "مالک")]
        public string Owner { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "مسیر سایت")]
        public string SiteUrl { get; set; }

        [Display(Name = "ساعات کاری")]
        public string WorkTime { get; set; }

        [Display(Name = "تلفن یک")]
        public string Tell1 { get; set; }

        [Display(Name = "تلفن دو")]
        public string Tell2 { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "آدرس اکانت Twitter")]
        public string Twitter { get; set; }

        [Display(Name = "آدرس اکانت Facebook")]
        public string Facebook { get; set; }

        [Display(Name = "آدرس اکانت Skype")]
        public string Skype { get; set; }

        [Display(Name = "آدرس اکانت Pinterest")]
        public string Pinterest { get; set; }

        [Display(Name = "آدرس اکانت Telegram")]
        public string Telegram { get; set; }

        [Display(Name = "آدرس اکانت Instagram")]
        public string Instagram { get; set; }
    }
}
