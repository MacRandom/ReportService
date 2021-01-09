using System.Collections.Generic;
using System.Threading.Tasks;
using ReportService.Models;

namespace ReportService.Abstract
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployeesByDepartmentId(int id);
    }
}
