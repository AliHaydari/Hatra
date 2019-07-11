using System;
using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Mvc;

namespace Hatra.ViewModels.VisitorsStatistics
{
    public class VisitorsStatisticsViewModel
    {
        public VisitorsStatisticsViewModel()
        {

        }

        public VisitorsStatisticsViewModel(Entities.VisitorsStatistics visitorsStatistics)
        {
            Id = visitorsStatistics.Id;
            UserAgent = visitorsStatistics.UserAgent;
            UserOs = visitorsStatistics.UserOs;
            BrowserName = visitorsStatistics.BrowserName;
            DeviceName = visitorsStatistics.DeviceName;
            IpAddress = visitorsStatistics.IpAddress;
            PageViewed = visitorsStatistics.PageViewed;
            VisitDate = visitorsStatistics.VisitDate;
        }

        [HiddenInput]
        public long Id { get; set; }

        [Display(Name = "User-Agent")]
        public string UserAgent { get; set; }

        [Display(Name = "سیستم عامل کاربر")]
        public string UserOs { get; set; }

        [Display(Name = "نام مرورگر")]
        public string BrowserName { get; set; }

        [Display(Name = "نام دستگاه")]
        public string DeviceName { get; set; }

        [Display(Name = "آدرس IP")]
        public string IpAddress { get; set; }

        [Display(Name = "صفحه بازدید شده")]
        public string PageViewed { get; set; }

        [Display(Name = "تاریخ بازدید")]
        public DateTimeOffset VisitDate { get; set; }

        public string VisitPersianDate => VisitDate.ToFriendlyPersianDateTextify();
    }
}
