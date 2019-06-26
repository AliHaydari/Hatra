using cloudscribe.Web.Pagination;
using System.Collections.Generic;

namespace Hatra.ViewModels.Paged
{
    public class PagedAdminBrandViewModel
    {
        public PagedAdminBrandViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<BrandViewModel> BrandViewModels { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
