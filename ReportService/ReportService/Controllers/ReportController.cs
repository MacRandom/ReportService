using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReportService.Abstract;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IEmployeeReportBuilder _employeeReportBuilder;

        public ReportController(IEmployeeReportBuilder employeeReportBuilder)
        {
            _employeeReportBuilder = employeeReportBuilder;
        }

        [HttpGet]
        [Route("{year}/{month}")]
        public async Task<IActionResult> Download(int year, int month)
        {
            string report = await _employeeReportBuilder.BuildAsync(year, month);

            return File(Encoding.UTF8.GetBytes(report), "text/plain", "report.txt");
        }
    }
}
