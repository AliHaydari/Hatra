using cloudscribe.Web.Pagination;
using System.Collections.Generic;

namespace Hatra.ViewModels.Paged
{
    public class PagedAdminPageViewModel
    {
        public PagedAdminPageViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<PageViewModel> PageViewModels { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
