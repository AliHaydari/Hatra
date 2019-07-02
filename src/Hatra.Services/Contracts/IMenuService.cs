using Hatra.Entities;
using Hatra.ViewModels;
using Hatra.ViewModels.Paged;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface IMenuService
    {
        Task<List<Menu>> GetAllForExcelExportAsync();
        Task<List<MenuViewModel>> GetAllAsync();
        Task<PagedAdminMenuViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage);
        Task<List<DropDownMenuViewModel>> GetAllParentAsync();
        Task<MenuViewModel> GetByIdAsync(int id);
        Task<bool> InsertAsync(MenuViewModel viewModel);
        Task<bool> UpdateAsync(MenuViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistNameAsync(int? id, string name);
        Task<bool> CheckExistRelationAsync(int id);
        Task<int> GetNextOrder();
    }
}
