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
        Task<List<ReferrerViewModel>> GetAllReferrerAsync();
        Task<GeneralStatisticsViewModel> GetGeneralStatisticsAsync(DateTimeOffset dt);
        Task<VisitorsStatisticsInRangeDateViewModel> GetInRangeDateAsync(DateTimeOffset fromDate, DateTimeOffset toDate);

        Task<bool> InsertAsync(VisitorsStatisticsViewModel viewModel);
    }
}
