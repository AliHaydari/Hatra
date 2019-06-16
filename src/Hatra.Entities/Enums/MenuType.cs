using System.ComponentModel.DataAnnotations;

namespace Hatra.Entities.Enums
{
    public enum MenuType
    {
        [Display(Name = "بالای صفحه")]
        Top,
        [Display(Name = "پایین صفحه")]
        Footer,
        [Display(Name = "موبایل")]
        Mobile
    }
}
