using System.Collections.Generic;

namespace Hatra.ViewModels
{
    public class SidebarWidgetViewModel
    {
        public SidebarWidgetViewModel()
        {
            
        }

        public int? ActiveCategoryId { get; set; }

        public List<CategoryViewModel> CategoryViewModels { get; set; }

        public List<PageViewModel> LastPageViewModels { get; set; }
    }
}
