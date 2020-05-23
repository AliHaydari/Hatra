using Hatra.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface IHardwareLockService
    {
        Task<HardwareLockResponseViewModel> InsertOrUpdateAsync(HardwareLockViewModel viewModel);
    }
}
