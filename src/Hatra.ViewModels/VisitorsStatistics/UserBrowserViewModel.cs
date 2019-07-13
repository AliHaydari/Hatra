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
                    case "IE":
                        _icon = "fab fa-internet-explorer";
                        break;

                    case "Firefox":
                    case "Firefox Mobile":
                    case "FireFox":
                    case "FireFox Mobile":
                    case "Mozilla":
                        _icon = "fab fa-firefox";
                        break;

                    case "Chrome":
                    case "Chrome Mobile":
                        _icon = "fab fa-chrome";
                        break;

                    case "Edge":
                    case "Edge Mobile":
                        _icon = "fab fa-edge";
                        break;

                    case "Opera":
                    case "Opera Mobile":
                    case "Opera Mini":
                    case "Opera Touch":
                        _icon = "fab fa-opera";
                        break;

                    default:
                        _icon = "fas fa-question-circle";
                        break;
                }
            }
        }

        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "تعداد بازدید")]
        public long ViewCount { get; set; }
    }
}
