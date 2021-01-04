using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ReportService.Domain;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        [HttpGet]
        [Route("{year}/{month}")]
        public IActionResult Download(int year, int month)
        {
            var actions = new List<(Action<Employee, Report>, Employee)>();
            var report = new Report() { ReportText = MonthNameResolver.MonthName.GetName(year, month) };
            var connectionString = "Host=192.168.99.100;Username=postgres;Password=1;Database=employee";
            

            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            var departmentCommand = new NpgsqlCommand("SELECT d.name from deps d where d.active = true", conn);
            var departmentReader = departmentCommand.ExecuteReader();
            while (departmentReader.Read())
            {
                List<Employee> employees = new List<Employee>();
                var departmentName = departmentReader.GetString(0);
                var conn1 = new NpgsqlConnection(connectionString);
                conn1.Open();
                var employeeCommand = new NpgsqlCommand("SELECT e.name, e.inn, d.name from emps e left join deps d on e.departmentid = d.id", conn1);
                var employeeReader = employeeCommand.ExecuteReader();
                while (employeeReader.Read())
                {
                    var employee = new Employee() { Name = employeeReader.GetString(0), Inn = employeeReader.GetString(1), Department = employeeReader.GetString(2) };
                    employee.BuhCode = EmpCodeResolver.GetCode(employee.Inn).Result;
                    employee.Salary = employee.Salary();
                    if (employee.Department != departmentName)
                        continue;
                    employees.Add(employee);
                }

                actions.Add((new ReportFormatter(null).NL, new Employee()));
                actions.Add((new ReportFormatter(null).WL, new Employee()));
                actions.Add((new ReportFormatter(null).NL, new Employee()));
                actions.Add((new ReportFormatter(null).WD, new Employee() { Department = departmentName } ));
                for (int i = 1; i < employees.Count(); i ++)
                {
                    actions.Add((new ReportFormatter(employees[i]).NL, employees[i]));
                    actions.Add((new ReportFormatter(employees[i]).WE, employees[i]));
                    actions.Add((new ReportFormatter(employees[i]).WT, employees[i]));
                    actions.Add((new ReportFormatter(employees[i]).WS, employees[i]));
                }  

            }
            actions.Add((new ReportFormatter(null).NL, null));
            actions.Add((new ReportFormatter(null).WL, null));

            foreach (var act in actions)
            {
                act.Item1(act.Item2, report);
            }
            report.Save();
            var file = System.IO.File.ReadAllBytes("D:\\report.txt");
            var response = File(file, "application/octet-stream", "report.txt");
            return response;
        }
    }
}
