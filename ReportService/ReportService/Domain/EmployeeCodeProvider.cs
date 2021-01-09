﻿using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ReportService.Abstract;

namespace ReportService.Domain
{
    public class EmployeeCodeProvider : IEmployeeCodeProvider
    {
        private readonly string _url;

        public EmployeeCodeProvider(IConfiguration configuration)
        {
            _url = configuration["ExternalApi:BuhCodeApiUrl"];
        }

        public async Task<string> GetCodeAsync(string inn)
        {
            var client = new HttpClient();

            return await client.GetStringAsync(_url + inn);
        }
    }
}
