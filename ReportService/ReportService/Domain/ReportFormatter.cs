using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Domain
{
    public class ReportFormatter
    {
        public ReportFormatter(Employee e)
        {
            Employee = e;
        }

        public Action<Employee, Report> NL = (e, s) => s.ReportText = s.ReportText + Environment.NewLine;
        public Action<Employee, Report> WL = (e, s) => s.ReportText = s.ReportText + "--------------------------------------------";
        public Action<Employee, Report> WT = (e, s) => s.ReportText = s.ReportText + "         ";
        public Action<Employee, Report> WE = (e, s) => s.ReportText = s.ReportText + e.Name;
        public Action<Employee, Report> WS = (e, s) => s.ReportText = s.ReportText + e.Salary + "р";
        public Action<Employee, Report> WD = (e, s) => s.ReportText = s.ReportText + e.Department;
        public Employee Employee { get; }
    }
}
