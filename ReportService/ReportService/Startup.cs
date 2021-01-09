using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.Abstract;
using ReportService.Domain;
using ReportService.Models;

namespace ReportService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmployeeCodeProvider, EmployeeCodeProvider>();
            services.AddTransient<IEmployeeSalaryProvider, EmployeeSalaryProvider>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeReportBuilder, EmployeeReportBuilder>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(errorApp =>
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "text/plain; charset=utf-8";

                    var error = context.Features.Get<IExceptionHandlerFeature>();

                    if (error != null)
                    {
                        var exc = error.Error;

                        await context.Response.WriteAsync(exc.Message, Encoding.UTF8);
                    }
                }));

            app.UseMvc();
        }
    }
}
