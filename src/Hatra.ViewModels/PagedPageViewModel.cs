using cloudscribe.Web.Pagination;
using System.Collections.Generic;

namespace Hatra.ViewModels
{
    public class PagedPageViewModel
    {
        public PagedPageViewModel()
        {
            Paging = new PaginationSettings();
        }

        public CategoryViewModel CategoryViewModel { get; set; }

        public List<PageViewModel> PageViewModels { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
