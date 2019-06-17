using EFSecondLevelCache.Core;
using Hatra.Common.GuardToolkit;
using Hatra.DataLayer.Context;
using Hatra.Entities;
using Hatra.Services.Contracts;
using Hatra.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hatra.Services
{
    public class FolderService : IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Folder> _folders;

        public FolderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _folders = _unitOfWork.Set<Folder>();
            _folders.CheckArgumentIsNull(nameof(_folders));
        }

        public async Task<List<FolderViewModel>> GetAllAsync()
        {
            return await _folders
                .Select(p => new FolderViewModel(p))
                .Cacheable()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<FolderViewModel> GetByIdAsync(int id)
        {
            var entity = await _folders.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                return new FolderViewModel(entity);
            }

            return null;
        }

        public async Task<bool> InsertAsync(FolderViewModel viewModel)
        {
            var entity = new Folder()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
            };

            await _folders.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(FolderViewModel viewModel)
        {
            var entity = await _folders.FirstOrDefaultAsync(p => p.Id == viewModel.Id);

            if (entity != null)
            {
                entity.Name = viewModel.Name;

                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _folders.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                _folders.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CheckExistAsync(int id)
        {
            return await _folders.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CheckExistNameAsync(int? id, string name)
        {
            return id == null
                ? await _folders.AnyAsync(p => p.Name == name)
                : await _folders.AnyAsync(p => p.Id != id && p.Name == name);
        }

        public async Task<bool> CheckExistRelationAsync(int id)
        {
            var result = await _folders
                .Include(p => p.Pictures)
                .Where(p => p.Id == id)
                .AnyAsync(p => p.Pictures.Any());

            return await Task.FromResult(result);
        }
    }
}
