using System.Net.Http;
using System.Threading.Tasks;
using ReportService.Abstract;

namespace ReportService.Domain
{
    public class EmployeeCodeProvider : IEmployeeCodeProvider
    {
        private const string url = @"http://buh.local/api/inn/";
        public async Task<string> GetCodeAsync(string inn)
        {
            var client = new HttpClient();

            return await client.GetStringAsync(url + inn);
        }
    }
}
