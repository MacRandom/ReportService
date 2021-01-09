using System;
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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;
        private readonly IEmployeeCodeProvider _employeeCodeProvider;
        private readonly IEmployeeSalaryProvider _employeeSalaryProvider;

        public EmployeeRepository(IConfiguration configuration, IEmployeeCodeProvider employeeCodeProvider, IEmployeeSalaryProvider employeeSalaryProvider)
        {
            _connectionString = configuration["ConnectionStrings:Default"];
            _employeeCodeProvider = employeeCodeProvider;
            _employeeSalaryProvider = employeeSalaryProvider;
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
                employee.BuhCode = await _employeeCodeProvider.GetCodeAsync(employee.Inn);

                string salaryString = await _employeeSalaryProvider.GetSalaryAsync(employee.Inn, employee.BuhCode);

                if (decimal.TryParse(salaryString, out decimal salary))
                    employee.Salary = Convert.ToInt32(salary);
            }

            return employees;
        }
    }
}
