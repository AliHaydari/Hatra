using System;
using System.Linq;
using System.Linq.Expressions;
using Hatra.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hatra.Services.Contracts
{
    public interface IEntityInformationService
    {
        Task<AuditableInformationViewModel> GetAuditableInformationByIdAsync(int id);
    }

    //public abstract class EntityInformation2Service<T> where T : DbSet<T>
    //{
    //    public async Task<AuditableViewModel> GetAuditableInformationByIdAsync(object id, Expression<Func<T, int, bool>> expression)
    //    {
    //        var query = Activator.CreateInstance<T>()
    //            .Where(expression)
    //            .Select(p => new AuditableViewModel()
    //            {
    //                CreatedByBrowserName = EF.Property<string>(p, nameof(AuditableViewModel.CreatedByBrowserName)),
    //                ModifiedByBrowserName = EF.Property<string>(p, nameof(AuditableViewModel.ModifiedByBrowserName)),
    //                CreatedByIp = EF.Property<string>(p, nameof(AuditableViewModel.CreatedByIp)),
    //                ModifiedByIp = EF.Property<string>(p, nameof(AuditableViewModel.ModifiedByIp)),
    //                CreatedByUserId = EF.Property<int?>(p, nameof(AuditableViewModel.CreatedByUserId)),
    //                ModifiedByUserId = EF.Property<int?>(p, nameof(AuditableViewModel.ModifiedByUserId)),
    //                CreatedDateTime = EF.Property<DateTimeOffset?>(p, nameof(AuditableViewModel.CreatedDateTime)),
    //                ModifiedDateTime = EF.Property<DateTimeOffset?>(p, nameof(AuditableViewModel.ModifiedDateTime)),
    //            })
    //            .AsNoTracking();

    //        return await query.FirstOrDefaultAsync();
    //    }
    //}
}
