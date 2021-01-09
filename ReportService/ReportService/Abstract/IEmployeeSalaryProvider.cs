using System.Threading.Tasks;

namespace ReportService.Abstract
{
    public interface IEmployeeSalaryProvider
    {
        Task<string> GetSalaryAsync(string inn, string buhCode);
    }
}
