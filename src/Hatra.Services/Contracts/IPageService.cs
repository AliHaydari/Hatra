using Hatra.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface IPageService
    {
        Task<List<PageViewModel>> GetAllAsync();
        Task<PageViewModel> GetByIdAsync(int id);
        Task<PageViewModel> GetByIdAndUpdateViewNumberAsync(int id);
        Task<bool> InsertAsync(PageViewModel viewModel);
        Task<bool> UpdateAsync(PageViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistTitleAsync(int? id, string title);
        Task<bool> CheckExistRelationAsync(int id);
        Task UpdateViewNumber(int id);
    }
}
