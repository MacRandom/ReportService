﻿using System;
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
            var actions = new Dictionary<Employee, Action<Employee, string>>();
            string report = MonthNameResolver.MonthName.GetName(year, month);
            var connString = "Host=127.0.0.1;Username=postgres;Password=1;Database=employee";
            List<Employee> emplist = new List<Employee>();

            var conn = new NpgsqlConnection(connString);
            conn.Open();
            var cmd = new NpgsqlCommand("SELECT d.name from deps where d.active = true", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var depName = reader.GetString(0);
                var conn1 = new NpgsqlConnection(connString);
                conn1.Open();
                var cmd1 = new NpgsqlCommand("SELECT e.name, e.inn, d.name from emps e left join deps d on e.departmentid = d.id", conn);
                var reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    var emp = new Employee() { Name = reader.GetString(0), Inn = reader.GetString(1), Department = reader.GetString(2) };
                    emp.BuhCode = EmpCodeResolver.GetCode(emp.Inn).Result;
                    emp.Salary = emp.Salary();
                    emplist.Add(emp);
                    if (emp.Department != depName)
                        continue;
                    emplist.Add(emp);
                }

                actions.Add(null, new ReportFormatter(null).NL);
                actions.Add(null, new ReportFormatter(null).WL);
                actions.Add(new Employee() { Department = depName } , new ReportFormatter(null).WD);
                for (int i = 1; i < emplist.Count(); i ++)
                {
                    actions.Add(emplist[i], new ReportFormatter(emplist[i]).NL);
                    actions.Add(emplist[i], new ReportFormatter(emplist[i]).WE);
                    actions.Add(emplist[i], new ReportFormatter(emplist[i]).WT);
                    actions.Add(emplist[i], new ReportFormatter(emplist[i]).WS);
                }  

            }
            actions.Add(null, new ReportFormatter(null).NL);
            actions.Add(null, new ReportFormatter(null).WL);

            foreach (var act in actions)
            {
                act.Value(act.Key, report);
            }
            System.IO.File.WriteAllText("C:\\report.txt", report);
            var file = System.IO.File.ReadAllBytes("C:\\report.txt");
            var response = File(file, "application/octet-stream", "report.txt");
            return response;
        }
    }
}
