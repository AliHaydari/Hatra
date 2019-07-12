using Hatra.Common.WebToolkit;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels.VisitorsStatistics
{
    public class UserBrowserViewModel
    {
        public UserBrowserViewModel()
        {
            
        }

        public UserBrowserViewModel(string userAgent)
        {
            var clientInfo = VisitorsStatisticsHelper.GetUserBrowserName(userAgent);

            Icon = clientInfo.Family.ToLowerInvariant();
            Name = clientInfo.ToString();
        }

        private string _icon;
        [Display(Name = "آیکون")]
        public string Icon {
            get => _icon;
            set {
                switch (value)
                {
                    case "InternetExplorer":
                        _icon = "internet-explorer";
                        break;
                    case "IE":
                        _icon = "internet-explorer";
                        break;
                    case "Unknown":
                        _icon = "question-circle";
                        break;
                    case "Mozilla":
                        _icon = "firefox";
                        break;
                    default:
                        _icon = value;
                        break;
                }
            }
        }

        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "تعداد بازدید")]
        public long ViewCount { get; set; }

        [Display(Name = "بازدید کل")]
        public long TotalVisits { get; set; }
    }
}
