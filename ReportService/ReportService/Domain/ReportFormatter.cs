using System;

namespace ReportService.Domain
{
    public class ReportFormatter
    {
        public Employee Employee
        {
            get;
            private set;
        }

        public ReportFormatter(Employee employee)
        {
            Employee = employee;
        }

        public Action<Employee, Report> NewLine =
            (e, s) => s.ReportText += Environment.NewLine;

        public Action<Employee, Report> WriteLine =
            (e, s) => s.ReportText += "--------------------------------------------";

        public Action<Employee, Report> WriteTab =
            (e, s) => s.ReportText += "         ";

        public Action<Employee, Report> WriteEmployee =
            (e, s) => s.ReportText += e.Name;

        public Action<Employee, Report> WriteSalary =
            (e, s) => s.ReportText += e.Salary + "р";

        public Action<Employee, Report> WriteDepartment =
            (e, s) => s.ReportText += e.Department;
    }
}
