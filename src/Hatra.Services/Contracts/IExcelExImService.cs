using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hatra.Services.Contracts
{
    public interface IExcelExImService<T>
    {
        List<T> ExportToExcel();
        Task<List<T>> ExportToExcelAsync();

        int ImportFromExcel(List<T> list);
        Task<int> ImportFromExcelAsync(List<T> list);
    }
}
