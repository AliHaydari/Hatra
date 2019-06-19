using Hatra.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface IPageService
    {
        Task<List<PageViewModel>> GetAllAsync();
        Task<List<PageViewModel>> GetAllWithoutCategoryAsync();
        Task<List<DropDownMenuViewModel>> GetAllWithoutCategoryDropDownMenuAsync();
        Task<List<DropDownMenuViewModel>> GetAllVisibleWithoutCategoryDropDownMenuAsync();
        Task<List<PageViewModel>> GetAllByCategoryIdAsync(int categoryId);
        Task<List<PageViewModel>> GetAllVisibleByCategoryIdAsync(int categoryId);
        Task<PagedPageViewModel> GetAllPagedVisibleByCategoryIdAsync(int categoryId, int pageNumber, int recordsPerPage);
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
