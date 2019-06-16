using Hatra.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface ISlideShowService
    {
        Task<List<SlideShowViewModel>> GetAllAsync();
        Task<SlideShowViewModel> GetByIdAsync(int id);
        Task<bool> InsertAsync(SlideShowViewModel viewModel);
        Task<bool> UpdateAsync(SlideShowViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistTitleAsync(int? id, string title);
    }
}
