﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ReportService.Abstract;

namespace ReportService.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private string _connectionString = "Host=192.168.99.100;Username=postgres;Password=1;Database=employee";
        private IEmployeeCodeProvider _employeeCodeProvider;
        private IEmployeeSalaryProvider _employeeSalaryProvider;

        public EmployeeRepository(IEmployeeCodeProvider employeeCodeProvider, IEmployeeSalaryProvider employeeSalaryProvider)
        {
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
