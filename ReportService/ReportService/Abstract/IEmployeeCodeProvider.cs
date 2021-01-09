using System.Threading.Tasks;

namespace ReportService.Abstract
{
    public interface IEmployeeCodeProvider
    {
        Task<string> GetCodeAsync(string inn);
    }
}
