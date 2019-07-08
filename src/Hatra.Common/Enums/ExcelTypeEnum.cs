using System.ComponentModel.DataAnnotations;

namespace Hatra.Common.Enums
{
    public enum ExcelTypeEnum
    {
        /// <summary>
        /// منو
        /// </summary>
        [Display(Name = "منو")]
        Menus,

        /// <summary>
        /// اسلاید شو
        /// </summary>
        [Display(Name = "اسلاید شو")]
        SlideShows,

        /// <summary>
        /// برند
        /// </summary>
        [Display(Name = "برند")]
        Brands,

        /// <summary>
        /// گروه
        /// </summary>
        [Display(Name = "گروه")]
        Categories,

        /// <summary>
        /// صفحه
        /// </summary>
        [Display(Name = "صفحه")]
        Pages,

        /// <summary>
        /// لینک مفید
        /// </summary>
        [Display(Name = "لینک مفید")]
        UsefulLinks,
    }
}
