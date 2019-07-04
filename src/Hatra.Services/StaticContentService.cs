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
    public class StaticContentService : IStaticContentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<StaticContent> _staticContents;

        public StaticContentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _staticContents = _unitOfWork.Set<StaticContent>();
            _staticContents.CheckArgumentIsNull(nameof(_staticContents));
        }

        public async Task<List<StaticContentViewModel>> GetAllAsync()
        {
            return await _staticContents
                .Select(p => new StaticContentViewModel(p))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedAdminStaticContentViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage)
        {
            var skipRecords = pageNumber * recordsPerPage;

            var query = _staticContents
                .OrderByDescending(p => p.Id)
                .Select(p => new StaticContentViewModel(p))
                .AsNoTracking();

            return new PagedAdminStaticContentViewModel()
            {
                Paging =
                {
                    TotalItems = await query.CountAsync(),
                },

                StaticContentViewModels = await query.Skip(skipRecords).Take(recordsPerPage).ToListAsync(),
            };
        }

        public async Task<StaticContentViewModel> GetByIdAsync(int id)
        {
            var entity = await _staticContents
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                return new StaticContentViewModel(entity);
            }

            return null;
        }

        public async Task<StaticContentViewModel> GetByNameAsync(string name)
        {
            var entity = await _staticContents
                .FirstOrDefaultAsync(p => p.Name == name);

            if (entity != null)
            {
                return new StaticContentViewModel(entity);
            }

            return null;
        }

        public async Task<StaticContentViewModel> GetVisibleByIdAsync(int id)
        {
            var entity = await _staticContents
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                return new StaticContentViewModel(entity);
            }

            return null;
        }

        public async Task<bool> InsertAsync(StaticContentViewModel viewModel)
        {
            var entity = new StaticContent()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Content = viewModel.Content,
            };

            await _staticContents.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(StaticContentViewModel viewModel)
        {
            var entity = await _staticContents.FirstOrDefaultAsync(p => p.Id == viewModel.Id);

            if (entity != null)
            {
                entity.Name = viewModel.Name;
                entity.Content = viewModel.Content;

                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _staticContents.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                _staticContents.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CheckExistAsync(int id)
        {
            return await _staticContents.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CheckExistNameAsync(int? id, string name)
        {
            return id == null
                ? await _staticContents.AnyAsync(p => p.Name == name)
                : await _staticContents.AnyAsync(p => p.Id != id && p.Name == name);
        }
    }
}
