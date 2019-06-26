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
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Brand> _brands;

        public BrandService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _brands = _unitOfWork.Set<Brand>();
            _brands.CheckArgumentIsNull(nameof(_brands));
        }

        public async Task<List<BrandViewModel>> GetAllAsync()
        {
            return await _brands
                .Select(p => new BrandViewModel(p))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedAdminBrandViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage)
        {
            var skipRecords = pageNumber * recordsPerPage;

            var query = _brands
                .OrderByDescending(p => p.Id)
                .Select(p => new BrandViewModel(p))
                .AsNoTracking();

            return new PagedAdminBrandViewModel()
            {
                Paging =
                {
                    TotalItems = await query.CountAsync(),
                },

                BrandViewModels = await query.Skip(skipRecords).Take(recordsPerPage).ToListAsync(),
            };
        }

        public async Task<List<BrandViewModel>> GetAllVisibleAsync()
        {
            return await _brands
                .Where(p => p.IsShow)
                .Select(p => new BrandViewModel(p))
                .Cacheable()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BrandViewModel> GetByIdAsync(int id)
        {
            var entity = await _brands
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                return new BrandViewModel(entity);
            }

            return null;
        }

        public async Task<BrandViewModel> GetVisibleByIdAsync(int id)
        {
            var entity = await _brands
                .FirstOrDefaultAsync(p => p.Id == id && p.IsShow);

            if (entity != null)
            {
                return new BrandViewModel(entity);
            }

            return null;
        }

        public async Task<bool> InsertAsync(BrandViewModel viewModel)
        {
            var entity = new Brand()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Image = viewModel.Image,
                Link = viewModel.Link,
                Description = viewModel.Description,
                IsShow = viewModel.IsShow,
            };

            await _brands.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(BrandViewModel viewModel)
        {
            var entity = await _brands.FirstOrDefaultAsync(p => p.Id == viewModel.Id);

            if (entity != null)
            {
                entity.Name = viewModel.Name;
                entity.Image = viewModel.Image;
                entity.Link = viewModel.Link;
                entity.Description = viewModel.Description;
                entity.IsShow = viewModel.IsShow;

                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _brands.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                _brands.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CheckExistAsync(int id)
        {
            return await _brands.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CheckExistNameAsync(int? id, string name)
        {
            return id == null
                ? await _brands.AnyAsync(p => p.Name == name)
                : await _brands.AnyAsync(p => p.Id != id && p.Name == name);
        }
    }
}
