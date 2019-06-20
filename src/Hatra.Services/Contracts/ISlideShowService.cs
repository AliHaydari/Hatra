using Hatra.ViewModels;
using Hatra.ViewModels.Paged;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface ISlideShowService
    {
        Task<List<SlideShowViewModel>> GetAllAsync();
        Task<PagedAdminSlideShowViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage);
        Task<List<SlideShowViewModel>> GetAllVisibleAsync();
        Task<SlideShowViewModel> GetByIdAsync(int id);
        Task<bool> InsertAsync(SlideShowViewModel viewModel);
        Task<bool> UpdateAsync(SlideShowViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistTitleAsync(int? id, string title);
        Task<int> GetNextOrder();
    }
}
