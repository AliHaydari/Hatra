using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels.Excels
{
    public enum ExcelTypeEnum
    {
        [Display(Name = "منو")]
        Menus,

        [Display(Name = "اسلاید شو")]
        SlideShows,

        [Display(Name = "برند")]
        Brands,

        [Display(Name = "گروه")]
        Categories,

        [Display(Name = "صفحه")]
        Pages,
    }
}
