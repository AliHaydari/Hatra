using EFSecondLevelCache.Core;
using Hatra.Common.GuardToolkit;
using Hatra.DataLayer.Context;
using Hatra.Entities;
using Hatra.Services.Contracts;
using Hatra.ViewModels;
using Hatra.ViewModels.Paged;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hatra.Services
{
    public class SlideShowService : ISlideShowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<SlideShow> _slideShows;

        public SlideShowService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _slideShows = _unitOfWork.Set<SlideShow>();
            _slideShows.CheckArgumentIsNull(nameof(_slideShows));
        }

        public async Task<List<SlideShowViewModel>> GetAllAsync()
        {
            return await _slideShows
                .OrderBy(p => p.Order)
                .Select(p => new SlideShowViewModel(p))
                .Cacheable()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedAdminSlideShowViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage)
        {
            var skipRecords = pageNumber * recordsPerPage;

            var query = _slideShows
                .OrderBy(p => p.Order)
                .Select(p => new SlideShowViewModel(p))
                .AsNoTracking();

            return new PagedAdminSlideShowViewModel()
            {
                Paging =
                {
                    TotalItems = await query.CountAsync(),
                },

                SlideShowViewModels = await query.Skip(skipRecords).Take(recordsPerPage).ToListAsync(),
            };
        }

        public async Task<List<SlideShowViewModel>> GetAllVisibleAsync()
        {
            return await _slideShows
                .Where(p => p.IsShow)
                .OrderBy(p => p.Order)
                .Select(p => new SlideShowViewModel(p))
                .Cacheable()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SlideShowViewModel> GetByIdAsync(int id)
        {
            var entity = await _slideShows.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                return new SlideShowViewModel(entity);
            }

            return null;
        }

        public async Task<bool> InsertAsync(SlideShowViewModel viewModel)
        {
            var entity = new SlideShow()
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                BriefDescription = viewModel.BriefDescription,
                Description = viewModel.Description,
                Image = viewModel.Image,
                Link1 = viewModel.Link1,
                Link2 = viewModel.Link2,
                Order = viewModel.Order,
                IsShow = viewModel.IsShow,
            };

            await _slideShows.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(SlideShowViewModel viewModel)
        {
            var entity = await _slideShows.FirstOrDefaultAsync(p => p.Id == viewModel.Id);

            if (entity != null)
            {
                entity.Title = viewModel.Title;
                entity.BriefDescription = viewModel.BriefDescription;
                entity.Description = viewModel.Description;
                entity.Image = viewModel.Image;
                entity.Link1 = viewModel.Link1;
                entity.Link2 = viewModel.Link2;
                entity.Order = viewModel.Order;
                entity.IsShow = viewModel.IsShow;

                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _slideShows.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                _slideShows.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CheckExistAsync(int id)
        {
            return await _slideShows.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CheckExistTitleAsync(int? id, string title)
        {
            return id == null
                ? await _slideShows.AnyAsync(p => p.Title == title)
                : await _slideShows.AnyAsync(p => p.Id != id && p.Title == title);
        }

        public async Task<int> GetNextOrder()
        {
            return (await _slideShows.MaxAsync(p => (int?)p.Order)).GetValueOrDefault() + 1;
        }

    }
}
