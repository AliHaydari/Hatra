using System;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels.VisitorsStatistics
{
    public class RangeDateSelectedViewModel
    {
        [Display(Name = "از تاریخ")]
        public DateTimeOffset? FromDate { get; set; }

        [Display(Name = "تا تاریخ")]
        public DateTimeOffset? ToDate { get; set; }
    }
}
