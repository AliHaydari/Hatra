using EFSecondLevelCache.Core;
using Hatra.Common.GuardToolkit;
using Hatra.Common.WebToolkit;
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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Category> _categories;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _categories = _unitOfWork.Set<Category>();
            _categories.CheckArgumentIsNull(nameof(_categories));
        }

        public async Task<List<CategoryViewModel>> GetAllAsync()
        {
            return await _categories
                .Include(p => p.Pages)
                .Select(p => new CategoryViewModel(p))
                .Cacheable()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedAdminCategoryViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage)
        {
            var skipRecords = pageNumber * recordsPerPage;

            var query = _categories
                .Include(p => p.Pages)
                .OrderByDescending(p => p.Id)
                .Select(p => new CategoryViewModel(p))
                .Cacheable()
                .AsNoTracking();

            return new PagedAdminCategoryViewModel()
            {
                Paging =
                {
                    TotalItems = await query.CountAsync(),
                },

                CategoryViewModels = await query.Skip(skipRecords).Take(recordsPerPage).ToListAsync(),
            };
        }

        public async Task<List<CategoryViewModel>> GetAllVisibleAsync()
        {
            return await _categories
                .Include(p => p.Pages)
                .Where(p => p.IsShow)
                .Select(p => new CategoryViewModel(p))
                .Cacheable()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<DropDownMenuViewModel>> GetAllDropDownMenuAsync()
        {
            return await _categories
                .Select(p => new DropDownMenuViewModel()
                {
                    Id = p.Id,
                    Name = "گروه : " + p.Name,
                })
                .Cacheable()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<DropDownMenuViewModel>> GetAllVisibleDropDownMenuAsync()
        {
            return await _categories
                .Where(p => p.IsShow)
                .Select(p => new DropDownMenuViewModel()
                {
                    Id = p.Id,
                    Name = "گروه : " + p.Name,
                })
                .Cacheable()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CategoryViewModel> GetByIdAsync(int id)
        {
            var entity = await _categories
                .Include(p => p.Pages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                return new CategoryViewModel(entity);
            }

            return null;
        }

        public async Task<CategoryViewModel> GetByIdAndSlugUrlAsync(int id, string slugUrl)
        {
            var entity = await _categories
                .Include(p => p.Pages)
                .FirstOrDefaultAsync(p => p.Id == id && p.SlugUrl == slugUrl);

            if (entity != null)
            {
                return new CategoryViewModel(entity);
            }

            return null;
        }

        public async Task<CategoryViewModel> GetVisibleByIdAsync(int id)
        {
            var entity = await _categories
                .Include(p => p.Pages)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsShow);

            if (entity != null)
            {
                entity.Pages = entity.Pages.Where(p => p.IsShow).ToList();
                return new CategoryViewModel(entity);
            }

            return null;
        }

        public async Task<bool> InsertAsync(CategoryViewModel viewModel)
        {
            var entity = new Category()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                IsShow = viewModel.IsShow,
                SlugUrl = SeoHelpers.GenerateSlug(viewModel.Name),
            };

            await _categories.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(CategoryViewModel viewModel)
        {
            var entity = await _categories.FirstOrDefaultAsync(p => p.Id == viewModel.Id);

            if (entity != null)
            {
                entity.Name = viewModel.Name;
                entity.Description = viewModel.Description;
                entity.IsShow = viewModel.IsShow;
                entity.SlugUrl = SeoHelpers.GenerateSlug(viewModel.Name);

                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _categories.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                _categories.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CheckExistAsync(int id)
        {
            return await _categories.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CheckExistNameAsync(int? id, string name)
        {
            return id == null
                ? await _categories.AnyAsync(p => p.Name == name)
                : await _categories.AnyAsync(p => p.Id != id && p.Name == name);
        }

        public async Task<bool> CheckExistRelationAsync(int id)
        {
            var result = await _categories
                .Include(p => p.Pages)
                .Where(p => p.Id == id)
                .AnyAsync(p => p.Pages.Any());

            return await Task.FromResult(result);
        }
    }
}
