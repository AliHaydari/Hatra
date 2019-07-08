using EFSecondLevelCache.Core;
using Hatra.Common.GuardToolkit;
using Hatra.DataLayer.Context;
using Hatra.Entities;
using Hatra.Services.Contracts;
using Hatra.ViewModels;
using Hatra.ViewModels.Excels;
using Hatra.ViewModels.Paged;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hatra.Services
{
    public class UsefulLinkService : IUsefulLinkService, IExcelExImService<ExcelUsefulLinkViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<UsefulLink> _usefulLinks;

        public UsefulLinkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _usefulLinks = _unitOfWork.Set<UsefulLink>();
            _usefulLinks.CheckArgumentIsNull(nameof(_usefulLinks));
        }

        public async Task<List<UsefulLinkViewModel>> GetAllAsync()
        {
            return await _usefulLinks
                .Select(p => new UsefulLinkViewModel(p))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedAdminUsefulLinkViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage)
        {
            var skipRecords = pageNumber * recordsPerPage;

            var query = _usefulLinks
                .OrderByDescending(p => p.Id)
                .Select(p => new UsefulLinkViewModel(p))
                .AsNoTracking();

            return new PagedAdminUsefulLinkViewModel()
            {
                Paging =
                {
                    TotalItems = await query.CountAsync(),
                },

                UsefulLinkViewModels = await query.Skip(skipRecords).Take(recordsPerPage).ToListAsync(),
            };
        }

        public async Task<List<UsefulLinkViewModel>> GetAllVisibleAsync()
        {
            return await _usefulLinks
                .Where(p => p.IsShow)
                .Select(p => new UsefulLinkViewModel(p))
                .Cacheable()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UsefulLinkViewModel> GetByIdAsync(int id)
        {
            var entity = await _usefulLinks
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                return new UsefulLinkViewModel(entity);
            }

            return null;
        }

        public async Task<UsefulLinkViewModel> GetVisibleByIdAsync(int id)
        {
            var entity = await _usefulLinks
                .FirstOrDefaultAsync(p => p.Id == id && p.IsShow);

            if (entity != null)
            {
                return new UsefulLinkViewModel(entity);
            }

            return null;
        }

        public async Task<AuditableInformationViewModel> GetAuditableInformationByIdAsync(int id)
        {
            var query = _usefulLinks
                .Where(p => p.Id == id)
                .Select(p => new AuditableInformationViewModel()
                {
                    CreatedByBrowserName = EF.Property<string>(p, nameof(AuditableInformationViewModel.CreatedByBrowserName)),
                    ModifiedByBrowserName = EF.Property<string>(p, nameof(AuditableInformationViewModel.ModifiedByBrowserName)),
                    CreatedByIp = EF.Property<string>(p, nameof(AuditableInformationViewModel.CreatedByIp)),
                    ModifiedByIp = EF.Property<string>(p, nameof(AuditableInformationViewModel.ModifiedByIp)),
                    CreatedByUserId = EF.Property<int?>(p, nameof(AuditableInformationViewModel.CreatedByUserId)),
                    ModifiedByUserId = EF.Property<int?>(p, nameof(AuditableInformationViewModel.ModifiedByUserId)),
                    CreatedDateTime = EF.Property<DateTimeOffset?>(p, nameof(AuditableInformationViewModel.CreatedDateTime)),
                    ModifiedDateTime = EF.Property<DateTimeOffset?>(p, nameof(AuditableInformationViewModel.ModifiedDateTime)),
                })
                .AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> InsertAsync(UsefulLinkViewModel viewModel)
        {
            var entity = new UsefulLink()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Link = viewModel.Link,
                Order = viewModel.Order,
                Description = viewModel.Description,
                IsShow = viewModel.IsShow,
            };

            await _usefulLinks.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(UsefulLinkViewModel viewModel)
        {
            var entity = await _usefulLinks.FirstOrDefaultAsync(p => p.Id == viewModel.Id);

            if (entity != null)
            {
                entity.Name = viewModel.Name;
                entity.Link = viewModel.Link;
                entity.Order = viewModel.Order;
                entity.Description = viewModel.Description;
                entity.IsShow = viewModel.IsShow;

                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _usefulLinks.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                _usefulLinks.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CheckExistAsync(int id)
        {
            return await _usefulLinks.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CheckExistNameAsync(int? id, string name)
        {
            return id == null
                ? await _usefulLinks.AnyAsync(p => p.Name == name)
                : await _usefulLinks.AnyAsync(p => p.Id != id && p.Name == name);
        }

        public async Task<int> GetNextOrder()
        {
            return (await _usefulLinks.MaxAsync(p => (int?)p.Order)).GetValueOrDefault() + 1;
        }

        public List<ExcelUsefulLinkViewModel> ExportToExcel()
        {
            return _usefulLinks
                .OrderBy(p => p.Id)
                .Select(p => new ExcelUsefulLinkViewModel(p))
                .AsNoTracking()
                .ToList();
        }

        public async Task<List<ExcelUsefulLinkViewModel>> ExportToExcelAsync()
        {
            return await _usefulLinks
                .OrderBy(p => p.Id)
                .Select(p => new ExcelUsefulLinkViewModel(p))
                .AsNoTracking()
                .ToListAsync();
        }

        public int ImportFromExcel(List<ExcelUsefulLinkViewModel> list)
        {
            var entities = new List<UsefulLink>(list.Count);

            foreach (var viewModel in list)
            {
                var entity = new UsefulLink()
                {
                    Id = viewModel.Id,
                    Name = viewModel.Name,
                    Link = viewModel.Link,
                    Order = viewModel.Order,
                    Description = viewModel.Description,
                    IsShow = viewModel.IsShow,
                };

                entities.Add(entity);
            }

            _usefulLinks.AddRange(entities);
            var result = _unitOfWork.SaveChanges();
            return result;
        }

        public async Task<int> ImportFromExcelAsync(List<ExcelUsefulLinkViewModel> list)
        {
            var entities = new List<UsefulLink>(list.Count);

            foreach (var viewModel in list)
            {
                var entity = new UsefulLink()
                {
                    Id = viewModel.Id,
                    Name = viewModel.Name,
                    Link = viewModel.Link,
                    Order = viewModel.Order,
                    Description = viewModel.Description,
                    IsShow = viewModel.IsShow,
                };

                entities.Add(entity);
            }

            await _usefulLinks.AddRangeAsync(entities);
            var result = await _unitOfWork.SaveChangesAsync();
            return result;
        }

    }
}
