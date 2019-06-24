using Hatra.ViewModels;
using Hatra.ViewModels.Paged;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAllAsync();
        Task<PagedAdminCategoryViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage);
        Task<List<CategoryViewModel>> GetAllVisibleAsync();
        Task<List<DropDownMenuViewModel>> GetAllDropDownMenuAsync();
        Task<List<DropDownMenuViewModel>> GetAllVisibleDropDownMenuAsync();
        Task<CategoryViewModel> GetByIdAsync(int id);
        Task<CategoryViewModel> GetByIdAndSlugUrlAsync(int id, string slugUrl);
        Task<CategoryViewModel> GetVisibleByIdAsync(int id);
        Task<bool> InsertAsync(CategoryViewModel viewModel);
        Task<bool> UpdateAsync(CategoryViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistNameAsync(int? id, string name);
        Task<bool> CheckExistRelationAsync(int id);
    }
}