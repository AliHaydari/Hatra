using DNTPersianUtils.Core;
using Hatra.Common.GuardToolkit;
using Hatra.DataLayer.Context;
using Hatra.Entities;
using Hatra.Services.Contracts;
using Hatra.ViewModels.VisitorsStatistics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hatra.Services
{
    public class VisitorsStatisticsService : IVisitorsStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<VisitorsStatistics> _statistics;

        public VisitorsStatisticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _statistics = _unitOfWork.Set<VisitorsStatistics>();
            _statistics.CheckArgumentIsNull(nameof(_statistics));
        }

        public async Task<List<VisitorsStatisticsViewModel>> GetAllAsync()
        {
            return await _statistics
                .Select(p => new VisitorsStatisticsViewModel(p))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<long> GetTotalVisitsAsync()
        {
            var query = await _statistics
                .AsNoTracking()
                .LongCountAsync();

            return query;
        }

        public async Task<List<UserOsViewModel>> GetAllUserOsAsync()
        {
            var query = await _statistics
                .GroupBy(p => p.UserOs)
                .OrderByDescending(p => p.Count())
                .Select(p => new UserOsViewModel()
                {
                    Name = p.Key,
                    Icon = p.Key,
                    ViewCount = p.LongCount(),
                })
                .ToListAsync();

            return query;
        }

        public async Task<List<UserBrowserViewModel>> GetAllUserBrowserAsync()
        {
            var query = await _statistics
                .GroupBy(p => p.BrowserName)
                .OrderByDescending(p => p.Count())
                .Select(p => new UserBrowserViewModel()
                {
                    Name = p.Key,
                    Icon = p.Key,
                    ViewCount = p.LongCount(),
                })
                .ToListAsync();

            return query;
        }

        public async Task<List<PageViewViewModel>> GetAllPageViewAsync()
        {
            var query = await _statistics
                .GroupBy(p => p.PageViewed)
                .OrderByDescending(p => p.Count())
                .Select(p => new PageViewViewModel()
                {
                    PageUrl = p.Key,
                    ViewCount = p.LongCount(),
                })
                .ToListAsync();

            return query;
        }

        public async Task<List<ReferrerViewModel>> GetAllReferrerAsync()
        {
            var query = await _statistics
                .GroupBy(p => p.Referrer)
                .OrderByDescending(p => p.Count())
                .Select(p => new ReferrerViewModel()
                {
                    Referrer = p.Key,
                    ViewCount = p.LongCount(),
                })
                .ToListAsync();

            return query;
        }

        public async Task<GeneralStatisticsViewModel> GetGeneralStatisticsAsync(DateTimeOffset dt)
        {
            var todayVisits = await _statistics.LongCountAsync(p => p.VisitDate.Day == dt.Day);
            var yesterdayVisits = await _statistics.LongCountAsync(p => p.VisitDate.Day == dt.AddDays(-1).Day);

            var iranDateTime = dt.GetDateTimeOffsetPart(DateTimeOffsetPart.IranLocalDateTime);

            var startDateYear = iranDateTime.GetPersianYearStartAndEndDates().StartDate;
            var endDateYear = iranDateTime.GetPersianYearStartAndEndDates().EndDate;

            var startDateMonth = iranDateTime.GetPersianMonthStartAndEndDates().StartDate;
            var endDateMonth = iranDateTime.GetPersianMonthStartAndEndDates().EndDate;

            var thisMonth = await _statistics
                .LongCountAsync(p => p.VisitDate >= startDateMonth && p.VisitDate <= endDateMonth);
            var thisYear = await _statistics
                .LongCountAsync(p => p.VisitDate >= startDateYear && p.VisitDate <= endDateYear);

            var uniqueVisitors = await _statistics
                .GroupBy(p => p.IpAddress)
                .LongCountAsync();

            var peakDate = await _statistics
                .GroupBy(p => p.VisitDate.Date)
                .OrderByDescending(p => p.LongCount())
                .FirstOrDefaultAsync();

            var lowDate = await _statistics
                .GroupBy(p => p.VisitDate.Date)
                .OrderBy(p => p.LongCount())
                .FirstOrDefaultAsync();

            var viewModel = new GeneralStatisticsViewModel()
            {
                OnlineUsers = 0,
                TodayVisits = todayVisits,
                YesterdayVisits = yesterdayVisits,
                ThisMonthVisits = thisMonth,
                ThisYearVisits = thisYear,
                PeakDate = peakDate.Key.Date,
                LowDate = lowDate.Key.Date,
                TotalVisits = 0,
                UniqueVisitors = uniqueVisitors,
            };

            return viewModel;
        }

        public async Task<bool> InsertAsync(VisitorsStatisticsViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Referrer))
            {
                viewModel.Referrer = "Direct";
            }

            var entity = new VisitorsStatistics()
            {
                Id = viewModel.Id,
                UserAgent = viewModel.UserAgent,
                UserOs = viewModel.UserOs,
                BrowserName = viewModel.BrowserName,
                DeviceName = viewModel.DeviceName,
                IpAddress = viewModel.IpAddress,
                PageViewed = viewModel.PageViewed,
                Referrer = viewModel.Referrer,
                VisitDate = viewModel.VisitDate,
            };

            await _statistics.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }
    }
}
