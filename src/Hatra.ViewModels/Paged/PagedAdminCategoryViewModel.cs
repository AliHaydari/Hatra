using cloudscribe.Web.Pagination;
using System.Collections.Generic;

namespace Hatra.ViewModels.Paged
{
    public class PagedAdminCategoryViewModel
    {
        public PagedAdminCategoryViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<CategoryViewModel> CategoryViewModels { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
