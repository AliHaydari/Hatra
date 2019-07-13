using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels.VisitorsStatistics
{
    public class ReferrerViewModel
    {
        [Display(Name = "ارجاع دهنده")]
        public string Referrer { get; set; }

        [Display(Name = "تعداد بازدید")]
        public long ViewCount { get; set; }
    }
}
