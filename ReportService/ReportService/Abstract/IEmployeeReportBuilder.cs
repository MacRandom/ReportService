using System.Threading.Tasks;

namespace ReportService.Abstract
{
    public interface IEmployeeReportBuilder
    {
        Task<string> Build(int year, int month);
    }
}
