using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReportService.Abstract;

namespace ReportService.Domain
{
    public class EmployeeSalaryProvider : IEmployeeSalaryProvider
    {
        private readonly string _url;

        public EmployeeSalaryProvider(IConfiguration configuration)
        {
            _url = configuration["ExternalApi:SalaryApiUrl"];
        }

        public async Task<string> GetSalaryAsync(string inn, string buhCode)
        {
            var client = new HttpClient();
            var jsonString = JsonConvert.SerializeObject(new { buhCode });
            var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage result;

            try
            {
                result = await client.PostAsync(_url + inn, content);
            }
            catch (Exception exc)
            {
                throw new Exception("Ошибка подключения к сервису зарплат", exc);
            }

            return await result.Content.ReadAsStringAsync();
        }
    }
}
