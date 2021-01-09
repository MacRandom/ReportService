using System.Threading.Tasks;

namespace ReportService.Abstract
{
    public interface IEmployeeReportBuilder
    {
        Task<string> BuildAsync(int year, int month);
    }
}
