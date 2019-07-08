using Hatra.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface IPictureService
    {
        Task<List<PictureViewModel>> GetAllAsync();
        Task<List<PictureViewModel>> GetAllByFolderIdAsync(int folderId);
        Task<PictureViewModel> GetByIdAsync(int id);
        PictureViewModel GetByName(string name);
        Task<PictureViewModel> GetByNameAsync(string name);
        Task<AuditableInformationViewModel> GetAuditableInformationByIdAsync(int id);
        Task<bool> InsertAsync(PictureViewModel viewModel);
        Task<bool> InsertAllAsync(List<PictureViewModel> viewModels);
        Task<bool> UpdateAsync(PictureViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<(bool isSuccess, string pictureName)> DeleteInTupleAsync(int id);
    }
}
