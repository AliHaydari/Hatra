using cloudscribe.Web.Pagination;
using System.Collections.Generic;

namespace Hatra.ViewModels.Paged
{
    public class PagedAdminUsefulLinkViewModel
    {
        public PagedAdminUsefulLinkViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<UsefulLinkViewModel> UsefulLinkViewModels { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
