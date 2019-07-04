using cloudscribe.Web.Pagination;
using System.Collections.Generic;

namespace Hatra.ViewModels.Paged
{
    public class PagedAdminStaticContentViewModel
    {
        public PagedAdminStaticContentViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<StaticContentViewModel> StaticContentViewModels { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
