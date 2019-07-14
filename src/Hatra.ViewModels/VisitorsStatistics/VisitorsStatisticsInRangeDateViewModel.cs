using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels.VisitorsStatistics
{
    public class VisitorsStatisticsInRangeDateViewModel
    {
        [Display(Name = "پر بازدید ترین روز")]
        public DateTime? PeakDate { get; set; }

        [Display(Name = "کم بازدید ترین روز")]
        public DateTime? LowDate { get; set; }

        [Display(Name = "بازدید کل")]
        public long TotalVisits { get; set; }

        [Display(Name = "بازدید یونیک")]
        public long UniqueVisitors { get; set; }

        public List<UserBrowserViewModel> UserBrowserViewModels { get; set; }

        public List<UserOsViewModel> UserOsViewModels { get; set; }

        public List<PageViewViewModel> PageViewViewModels { get; set; }

        public List<ReferrerViewModel> ReferrerViewModels { get; set; }
    }
}
