using Hatra.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface IFolderService
    {
        Task<List<FolderViewModel>> GetAllAsync();
        Task<FolderViewModel> GetByIdAsync(int id);
        Task<bool> InsertAsync(FolderViewModel viewModel);
        Task<bool> UpdateAsync(FolderViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistNameAsync(int? id, string name);
        Task<bool> CheckExistRelationAsync(int id);
    }
}
