using cloudscribe.Web.Pagination;
using System.Collections.Generic;

namespace Hatra.ViewModels.Paged
{
    public class PagedAdminContactUsViewModel
    {
        public PagedAdminContactUsViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<ContactUsViewModel> ContactUsViewModels { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
