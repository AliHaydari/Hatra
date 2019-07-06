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
            Twitter = "http://twitter.com/Twitter-Account-Address";
            Facebook = "http://fb.com/Facebook-Account-Address";
            Skype = "http://skype.com/Skype-Account-Address";
            Pinterest = "http://pinterest.com/Pinterest-Account-Address";
            Telegram = "http://t.me/Telegram-Account-Address";
            Instagram = "http://instagram.com/Instagram-Account-Address";
            LinkedIn = "http://linkedin.com/LinkedIn-Account-Address";
            WhatsApp = "http://whatsapp.com/WhatsApp-Account-Address";
            Latitude = "0";
            Longitude = "0";
        }

        [Display(Name = "نام انگلیسی سایت")]
        [RegularExpression("^[a-zA-Z_\\s]*$", ErrorMessage = "لطفا تنها از حروف انگلیسی استفاده نمائید")]
        public string EnglishSiteName { get; set; }

        [Display(Name = "نام فارسی سایت")]
        [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$", ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
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
        [DataType(DataType.EmailAddress, ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید.")]
        [EmailAddress(ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید.")]
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
        [DataType(DataType.Url, ErrorMessage = "لطفا آدرس را به درستی وارد کنید.")]
        public string Twitter { get; set; }

        [Display(Name = "آدرس اکانت Facebook")]
        [DataType(DataType.Url, ErrorMessage = "لطفا آدرس را به درستی وارد کنید.")]
        public string Facebook { get; set; }

        [Display(Name = "آدرس اکانت Skype")]
        [DataType(DataType.Url, ErrorMessage = "لطفا آدرس را به درستی وارد کنید.")]
        public string Skype { get; set; }

        [Display(Name = "آدرس اکانت Pinterest")]
        [DataType(DataType.Url, ErrorMessage = "لطفا آدرس را به درستی وارد کنید.")]
        public string Pinterest { get; set; }

        [Display(Name = "آدرس اکانت Telegram")]
        [DataType(DataType.Url, ErrorMessage = "لطفا آدرس را به درستی وارد کنید.")]
        public string Telegram { get; set; }

        [Display(Name = "آدرس اکانت Instagram")]
        [DataType(DataType.Url, ErrorMessage = "لطفا آدرس را به درستی وارد کنید.")]
        public string Instagram { get; set; }

        [Display(Name = "آدرس اکانت LinkedIn")]
        [DataType(DataType.Url, ErrorMessage = "لطفا آدرس را به درستی وارد کنید.")]
        public string LinkedIn { get; set; }

        [Display(Name = "آدرس اکانت WhatsApp")]
        [DataType(DataType.Url, ErrorMessage = "لطفا آدرس را به درستی وارد کنید.")]
        public string WhatsApp { get; set; }

        [Display(Name = "عرض جغرافیایی")]
        public string Latitude { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public string Longitude { get; set; }
    }
}
