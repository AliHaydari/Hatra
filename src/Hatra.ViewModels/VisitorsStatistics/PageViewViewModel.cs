using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels.VisitorsStatistics
{
    public class PageViewViewModel
    {
        [Display(Name = "صفحه بازدید شده")]
        public string PageUrl { get; set; }

        [Display(Name = "تعداد بازدید")]
        public long ViewCount { get; set; }
    }
}
