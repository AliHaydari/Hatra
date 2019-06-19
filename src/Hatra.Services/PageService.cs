using Hatra.Common.GuardToolkit;
using Hatra.DataLayer.Context;
using Hatra.Entities;
using Hatra.Services.Contracts;
using Hatra.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hatra.Common.WebToolkit;

namespace Hatra.Services
{
    public class PageService : IPageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Page> _pages;

        public PageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _pages = _unitOfWork.Set<Page>();
            _pages.CheckArgumentIsNull(nameof(_pages));
        }

        public async Task<List<PageViewModel>> GetAllAsync()
        {
            return await _pages
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Select(p => new PageViewModel(p))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<PageViewModel>> GetAllWithoutCategoryAsync()
        {
            return await _pages
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.CategoryId == null)
                .Select(p => new PageViewModel(p))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<DropDownMenuViewModel>> GetAllWithoutCategoryDropDownMenuAsync()
        {
            return await _pages
                .Where(p => p.CategoryId == null)
                .Select(p => new DropDownMenuViewModel()
                {
                    Id = p.Id,
                    Name = "صفحه : " + p.Title,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<DropDownMenuViewModel>> GetAllVisibleWithoutCategoryDropDownMenuAsync()
        {
            return await _pages
                .Where(p => p.CategoryId == null && p.IsShow)
                .Select(p => new DropDownMenuViewModel()
                {
                    Id = p.Id,
                    Name = "صفحه : " + p.Title,
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<PageViewModel>> GetAllByCategoryIdAsync(int categoryId)
        {
            return await _pages
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new PageViewModel(p))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<PageViewModel>> GetAllVisibleByCategoryIdAsync(int categoryId)
        {
            return await _pages
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.IsShow && p.CategoryId == categoryId)
                .Select(p => new PageViewModel(p))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PageViewModel> GetByIdAsync(int id)
        {
            var entity = await _pages
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                return new PageViewModel(entity);
            }

            return null;
        }

        public async Task<PageViewModel> GetByIdAndUpdateViewNumberAsync(int id)
        {
            var entity = await _pages
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                entity.ViewNumber++;
                await _unitOfWork.SaveChangesAsync();

                return new PageViewModel(entity);
            }

            return null;
        }

        public async Task<bool> InsertAsync(PageViewModel viewModel)
        {
            var entity = new Page()
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                BriefDescription = viewModel.BriefDescription,
                Body = viewModel.Body,
                MetaDescription = viewModel.MetaDescription,
                SlugUrl = SeoHelpers.GenerateSlug(viewModel.Title),
                ViewNumber = 0,
                Image = viewModel.Image,
                Order = viewModel.Order,
                CategoryId = viewModel.CategoryId,
                IsShow = viewModel.IsShow,
            };

            await _pages.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(PageViewModel viewModel)
        {
            var entity = await _pages.FirstOrDefaultAsync(p => p.Id == viewModel.Id);

            if (entity != null)
            {
                entity.Title = viewModel.Title;
                entity.BriefDescription = viewModel.BriefDescription;
                entity.Body = viewModel.Body;
                entity.MetaDescription = viewModel.MetaDescription;
                entity.SlugUrl = SeoHelpers.GenerateSlug(viewModel.Title);
                entity.Image = viewModel.Image;
                entity.Order = viewModel.Order;
                entity.CategoryId = viewModel.CategoryId;
                entity.IsShow = viewModel.IsShow;

                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _pages.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                _pages.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CheckExistAsync(int id)
        {
            return await _pages.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CheckExistTitleAsync(int? id, string title)
        {
            return id == null
                ? await _pages.AnyAsync(p => p.Title == title)
                : await _pages.AnyAsync(p => p.Id != id && p.Title == title);
        }

        public async Task<bool> CheckExistRelationAsync(int id)
        {
            var result = await _pages
                .Include(p => p.Images)
                .Where(p => p.Id == id)
                .AnyAsync(p => p.Images.Any());

            return await Task.FromResult(result);
        }

        public async Task UpdateViewNumber(int id)
        {
            var entity = await _pages.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                entity.ViewNumber++;
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
