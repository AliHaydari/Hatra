using Hatra.Entities;
using Hatra.Entities.Enums;

namespace Hatra.ViewModels.Excels
{
    public class ExcelMenuViewModel
    {
        public ExcelMenuViewModel()
        {
            
        }

        public ExcelMenuViewModel(Menu menu)
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

        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        public int Order { get; set; }

        public MenuType Type { get; set; }

        public bool IsShow { get; set; }

        public bool IsMegaMenu { get; set; }
    }
}
