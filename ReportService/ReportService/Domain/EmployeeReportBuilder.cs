﻿using System.Text;
using System.Threading.Tasks;
using ReportService.Abstract;

namespace ReportService.Domain
{
    public class EmployeeReportBuilder : IEmployeeReportBuilder
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private const string _line = "--------------------------------------------";

        public EmployeeReportBuilder(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<string> BuildAsync(int year, int month)
        {
            _stringBuilder.AppendLine($"{MonthNameResolver.MonthName.GetName(year, month)} {year}");
            _stringBuilder.AppendLine(_line);

            int totalCompanySalary = 0;
            var departments = _departmentRepository.GetDepartments();
            foreach (var department in departments)
            {
                int totalDepartmentSalary = 0;
                _stringBuilder.AppendLine(department.Name);

                var employees = await _employeeRepository.GetEmployeesByDepartmentId(department.Id);
                foreach(var employee in employees)
                {
                    _stringBuilder.AppendLine($"{employee.Name} {employee.Salary}р");

                    totalDepartmentSalary += employee.Salary;
                }

                _stringBuilder.AppendLine($"Всего по отделу: {totalDepartmentSalary}р");
                _stringBuilder.AppendLine(_line);
                totalCompanySalary += totalDepartmentSalary;
            }
            _stringBuilder.AppendLine($"Всего по предприятию: {totalCompanySalary}р");
            return _stringBuilder.ToString();
        }
    }
}
