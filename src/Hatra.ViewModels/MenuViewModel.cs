using Hatra.Entities.Enums;
using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Hatra.Entities;

namespace Hatra.ViewModels
{
    public class MenuViewModel
    {
        public MenuViewModel()
        {

        }

        public MenuViewModel(Menu menu)
        {
            Id = menu.Id;
            Name = menu.Name;
            Link = menu.Link;
            ParentId = menu.ParentId;
            ParentName = menu.ParentMenu?.Name;
            Order = menu.Order;
            Type = menu.Type;
            IsShow = menu.IsShow;
            IsMegaMenu = menu.IsMegaMenu;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام")]
        [Remote("ValidateName", "Menus",
            AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id),
            HttpMethod = "POST")]
        [StringLength(450, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "لینک")]
        public string Link { get; set; }

        [Display(Name = "گروه پدر")]
        public int? ParentId { get; set; }

        [Display(Name = "نام گروه پدر")]
        public string ParentName { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "اولویت نمایش")]
        public int Order { get; set; }

        [Display(Name = "نوع")]
        public MenuType Type { get; set; }

        [Display(Name = "نمایش داده شود")]
        public bool IsShow { get; set; }

        [Display(Name = "مگا منو")]
        public bool IsMegaMenu { get; set; }
    }
}
