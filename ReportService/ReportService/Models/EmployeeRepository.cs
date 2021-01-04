using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using ReportService.Domain;

namespace ReportService.Models
{
    public class EmployeeRepository
    {
        private string _connectionString = "Host=192.168.99.100;Username=postgres;Password=1;Database=employee";

        public List<Employee> GetEmployeesByDepartmentId(int id)
        {
            var employees = new List<Employee>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                employees = db.Query<Employee>("SELECT e.name, e.inn from emps e where e.departmentid = @id", new { id }).ToList();
            }

            foreach (var employee in employees)
            {
                employee.BuhCode = EmployeeCodeResolver.GetCode(employee.Inn).Result;
                employee.Salary = employee.Salary();
            }

            return employees;
        }
    }
}
