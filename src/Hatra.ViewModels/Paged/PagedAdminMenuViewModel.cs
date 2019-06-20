using cloudscribe.Web.Pagination;
using System.Collections.Generic;

namespace Hatra.ViewModels.Paged
{
    public class PagedAdminMenuViewModel
    {
        public PagedAdminMenuViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<MenuViewModel> MenuViewModels { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
