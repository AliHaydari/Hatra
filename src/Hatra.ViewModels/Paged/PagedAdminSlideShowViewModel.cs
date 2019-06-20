using cloudscribe.Web.Pagination;
using System.Collections.Generic;

namespace Hatra.ViewModels.Paged
{
    public class PagedAdminSlideShowViewModel
    {
        public PagedAdminSlideShowViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<SlideShowViewModel> SlideShowViewModels { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
