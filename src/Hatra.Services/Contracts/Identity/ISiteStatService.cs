using System.Collections.Generic;
using System.Threading.Tasks;
using Hatra.Entities.Identity;
using System.Security.Claims;
using Hatra.ViewModels.Identity;

namespace Hatra.Services.Contracts.Identity
{
    public interface ISiteStatService
    {
        Task<List<User>> GetOnlineUsersListAsync(int numbersToTake, int minutesToTake);

        Task<List<User>> GetTodayBirthdayListAsync();

        Task UpdateUserLastVisitDateTimeAsync(ClaimsPrincipal claimsPrincipal);

        Task<AgeStatViewModel> GetUsersAverageAge();
    }
}