using Hatra.Common.WebToolkit;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels.VisitorsStatistics
{
    public class UserOsViewModel
    {
        public UserOsViewModel()
        {
            
        }

        public UserOsViewModel(string userAgent)
        {
            var clientInfo = VisitorsStatisticsHelper.GetUserOsName(userAgent);

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
                    case "Other":
                        _icon = "fas fa-question-circle";
                        break;
                    case "iOS":
                        _icon = "fab fa-apple";
                        break;
                    case "Mac OS X":
                        _icon = "fab fa-apple";
                        break;
                    case "Mac OS":
                        _icon = "fab fa-apple";
                        break;
                    case "Ubuntu":
                        _icon = "fab fa-ubuntu";
                        break;
                    case "Linux":
                        _icon = "fab fa-linux";
                        break;
                    case "Windows":
                        _icon = "fab fa-windows";
                        break;
                    case "Android":
                        _icon = "fab fa-android";
                        break;
                    default:
                        _icon = "fas fa-question-circle";
                        break;
                }
            }
        }

        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "بازدید کل")]
        public long ViewCount { get; set; }
    }
}
