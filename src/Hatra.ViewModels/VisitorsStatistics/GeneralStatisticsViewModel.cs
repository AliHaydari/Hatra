using System;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels.VisitorsStatistics
{
    public class GeneralStatisticsViewModel
    {
        [Display(Name = "افراد آنلاین")]
        public long OnlineUsers { get; set; }

        [Display(Name = "بازدید امروز")]
        public long TodayVisits { get; set; }

        [Display(Name = "بازدید دیروز")]
        public long YesterdayVisits { get; set; }

        [Display(Name = "بازدید این ماه")]
        public long ThisMonthVisits { get; set; }

        [Display(Name = "بازدید امسال")]
        public long ThisYearVisits { get; set; }

        [Display(Name = "پر بازدید ترین روز")]
        public DateTime? PeakDate { get; set; }

        [Display(Name = "کم بازدید ترین روز")]
        public DateTime? LowDate { get; set; }

        [Display(Name = "بازدید کل")]
        public long TotalVisits { get; set; }

        [Display(Name = "بازدید یونیک")]
        public long UniqueVisitors { get; set; }
    }
}
