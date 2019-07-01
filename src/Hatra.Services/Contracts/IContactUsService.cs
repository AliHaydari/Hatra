using Hatra.ViewModels;
using Hatra.ViewModels.Paged;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface IContactUsService
    {
        Task<List<ContactUsViewModel>> GetAllAsync();
        Task<PagedAdminContactUsViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage);
        Task<List<ContactUsViewModel>> GetAllAnsweredAsync();
        Task<ContactUsViewModel> GetByIdAsync(int id);
        Task<bool> InsertAsync(ContactUsViewModel viewModel);
        Task<bool> UpdateAsync(ContactUsViewModel viewModel);
        Task<bool> DeleteAsync(int id);
    }
}
