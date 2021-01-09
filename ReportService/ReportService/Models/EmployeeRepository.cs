using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ReportService.Abstract;

namespace ReportService.Models
{
    public class EmployeeRepository
    {
        private string _connectionString = "Host=192.168.99.100;Username=postgres;Password=1;Database=employee";
        private IEmployeeCodeProvider _employeeCodeResolver;

        public EmployeeRepository(IConfiguration configuration, IEmployeeCodeProvider employeeCodeResolver)
        {
            _employeeCodeResolver = employeeCodeResolver;
        }

        public async Task<List<Employee>> GetEmployeesByDepartmentId(int id)
        {
            var employees = new List<Employee>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                employees = db.Query<Employee>("SELECT e.name, e.inn from emps e where e.departmentid = @id", new { id }).ToList();
            }

            foreach (var employee in employees)
            {
                employee.BuhCode = await _employeeCodeResolver.GetCodeAsync(employee.Inn);
                employee.Salary = employee.Salary();
            }

            return employees;
        }
    }
}
