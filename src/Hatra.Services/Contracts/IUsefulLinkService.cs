using Hatra.ViewModels;
using Hatra.ViewModels.Paged;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface IUsefulLinkService
    {
        Task<List<UsefulLinkViewModel>> GetAllAsync();
        Task<PagedAdminUsefulLinkViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage);
        Task<List<UsefulLinkViewModel>> GetAllVisibleAsync();
        Task<UsefulLinkViewModel> GetByIdAsync(int id);
        Task<UsefulLinkViewModel> GetVisibleByIdAsync(int id);
        Task<AuditableInformationViewModel> GetAuditableInformationByIdAsync(int id);
        Task<bool> InsertAsync(UsefulLinkViewModel viewModel);
        Task<bool> UpdateAsync(UsefulLinkViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistNameAsync(int? id, string name);
        Task<int> GetNextOrder();
    }
}
