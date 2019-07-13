using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels.VisitorsStatistics
{
    public class StatisticsHelperViewModel<T> where T : class
    {
        [Display(Name = "بازدید کل")]
        public long TotalVisits { get; set; }

        public List<T> ViewModels { get; set; }
    }
}
