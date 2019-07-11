using Hatra.ViewModels.VisitorsStatistics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface IVisitorsStatisticsService
    {
        Task<List<VisitorsStatisticsViewModel>> GetAllAsync();
        Task<long> GetTotalVisitsAsync();
        Task<List<UserOsViewModel>> GetAllUserOsAsync();
        Task<List<UserBrowserViewModel>> GetAllUserBrowserAsync();
        Task<List<PageViewViewModel>> GetAllPageViewAsync();
        Task<GeneralStatisticsViewModel> GetGeneralStatisticsAsync(DateTimeOffset dt);

        Task<bool> InsertAsync(VisitorsStatisticsViewModel viewModel);
    }
}
