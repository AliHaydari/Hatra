using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels.VisitorsStatistics
{
    public class UserOsViewModel
    {
        private string _icon;
        [Display(Name = "آیکون")]
        public string Icon {
            get => _icon;
            set {
                switch (value)
                {
                    case "Other":
                        _icon = "question-circle";
                        break;
                    case "iOS":
                        _icon = "apple";
                        break;
                    case "Mac OS X":
                        _icon = "apple";
                        break;
                    case "Mac OS":
                        _icon = "apple";
                        break;
                    case "Ubuntu":
                        _icon = "linux";
                        break;
                    default:
                        _icon = value;
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
