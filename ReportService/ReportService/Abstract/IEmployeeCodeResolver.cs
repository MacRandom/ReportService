using System.Threading.Tasks;

namespace ReportService.Abstract
{
    public interface IEmployeeCodeResolver
    {
        Task<string> GetCode(string inn);
    }
}
