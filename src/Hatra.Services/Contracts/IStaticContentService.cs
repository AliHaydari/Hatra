using Hatra.ViewModels;
using Hatra.ViewModels.Paged;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface IStaticContentService
    {
        Task<List<StaticContentViewModel>> GetAllAsync();
        Task<PagedAdminStaticContentViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage);
        Task<StaticContentViewModel> GetByIdAsync(int id);
        Task<StaticContentViewModel> GetByNameAsync(string name);
        Task<StaticContentViewModel> GetVisibleByIdAsync(int id);
        Task<bool> InsertAsync(StaticContentViewModel viewModel);
        Task<bool> UpdateAsync(StaticContentViewModel viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
        Task<bool> CheckExistNameAsync(int? id, string name);
    }
}
