using System;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels.VisitorsStatistics
{
    public class CurrentVisitorViewModel
    {
        [Display(Name = "مرورگر")]
        public string Browser { get; set; }

        public string BrowserIcon { get; set; }

        [Display(Name = "IP")]
        public string IpAddress { get; set; }

        [Display(Name = "نام کشور")]
        public string CountryName { get; set; }

        [Display(Name = "سیستم عامل")]
        public string OsName { get; set; }

        public string OsIcon { get; set; }

        [Display(Name = "بازدید کل")]
        public long TotalVisits { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
