using Hatra.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAllAsync();
        Task<List<CategoryViewModel>> GetAllVisibleAsync();
        Task<List<DropDownMenuViewModel>> GetAllDropDownMenuAsync();
        Task<List<DropDownMenuViewModel>> GetAllVisibleDropDownMenuAsync();
        Task<CategoryViewModel> GetByIdAsync(int id);
        Task<CategoryViewModel> GetVisibleByIdAsync(int id);
        Task<bool> InsertAsync(CategoryViewModel viewModel);
        Task<bool> UpdateAsync(CategoryViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistNameAsync(int? id, string name);
        Task<bool> CheckExistRelationAsync(int id);
    }
}