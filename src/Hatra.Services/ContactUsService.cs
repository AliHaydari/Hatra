using Hatra.Common.GuardToolkit;
using Hatra.DataLayer.Context;
using Hatra.Entities;
using Hatra.Services.Contracts;
using Hatra.ViewModels;
using Hatra.ViewModels.Paged;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hatra.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<ContactUs> _contactUses;

        public ContactUsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _contactUses = _unitOfWork.Set<ContactUs>();
            _contactUses.CheckArgumentIsNull(nameof(_contactUses));
        }

        public async Task<List<ContactUsViewModel>> GetAllAsync()
        {
            return await _contactUses
                .OrderByDescending(p => EF.Property<DateTimeOffset>(p, "CreatedDateTime"))
                .Select(p => new ContactUsViewModel(p))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedAdminContactUsViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage)
        {
            var skipRecords = pageNumber * recordsPerPage;

            var query = _contactUses
                .OrderByDescending(p => EF.Property<DateTimeOffset>(p, "CreatedDateTime"))
                .Select(p => new ContactUsViewModel(p))
                .AsNoTracking();

            return new PagedAdminContactUsViewModel()
            {
                Paging =
                {
                    TotalItems = await query.CountAsync(),
                },

                ContactUsViewModels = await query.Skip(skipRecords).Take(recordsPerPage).ToListAsync(),
            };
        }

        public async Task<List<ContactUsViewModel>> GetAllAnsweredAsync()
        {
            return await _contactUses
                .Where(p => p.IsAnsered)
                .OrderBy(p => EF.Property<DateTimeOffset>(p, "CreatedDateTime"))
                .Select(p => new ContactUsViewModel(p))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ContactUsViewModel> GetByIdAsync(int id)
        {
            var entity = await _contactUses.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                return new ContactUsViewModel(entity);
            }

            return null;
        }

        public async Task<AuditableInformationViewModel> GetAuditableInformationByIdAsync(int id)
        {
            var query = _contactUses
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

        public async Task<bool> InsertAsync(ContactUsViewModel viewModel)
        {
            var entity = new ContactUs()
            {
                Id = viewModel.Id,
                FullName = viewModel.FullName,
                Email = viewModel.Email,
                Subject = viewModel.Subject,
                Description = viewModel.Description,
                IsAnsered = false,
                Answer = null,
            };

            await _contactUses.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(ContactUsViewModel viewModel)
        {
            var entity = await _contactUses.FirstOrDefaultAsync(p => p.Id == viewModel.Id);

            if (entity != null)
            {
                //entity.FullName = viewModel.FullName;
                //entity.Email = viewModel.Email;
                //entity.Subject = viewModel.Subject;
                //entity.Description = viewModel.Description;
                //entity.IsAnsered = viewModel.IsAnsered;
                entity.IsAnsered = true;
                entity.Answer = viewModel.Answer;

                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _contactUses.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                _contactUses.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

    }
}
