using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReportService.Abstract;

namespace ReportService.Domain
{
    public class EmployeeSalaryProvider : IEmployeeSalaryProvider
    {
        private const string url = @"http://salary.local/api/empcode/";

        public async Task<string> GetSalaryAsync(string inn, string buhCode)
        {
            var client = new HttpClient();
            var jsonString = JsonConvert.SerializeObject(new { buhCode });
            var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var result = await client.PostAsync(url + inn, content);

            return await result.Content.ReadAsStringAsync();
        }
    }
}
