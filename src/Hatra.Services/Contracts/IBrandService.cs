using Hatra.ViewModels;
using Hatra.ViewModels.Paged;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface IBrandService
    {
        Task<List<BrandViewModel>> GetAllAsync();
        Task<PagedAdminBrandViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage);
        Task<List<BrandViewModel>> GetAllVisibleAsync();
        Task<BrandViewModel> GetByIdAsync(int id);
        Task<BrandViewModel> GetVisibleByIdAsync(int id);
        Task<AuditableInformationViewModel> GetAuditableInformationByIdAsync(int id);
        Task<bool> InsertAsync(BrandViewModel viewModel);
        Task<bool> UpdateAsync(BrandViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistNameAsync(int? id, string name);
    }
}
