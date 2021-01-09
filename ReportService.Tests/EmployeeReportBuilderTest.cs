using System;
using System.Collections.Generic;
using System.Text;
using ReportService.Domain;
using ReportService.Abstract;
using ReportService.Models;
using NUnit.Framework;
using Moq;
using System.Threading.Tasks;

namespace ReportService.Tests
{
    public class EmployeeReportBuilderTest
    {
        private IDepartmentRepository _departmentRepository;
        private IEmployeeRepository _employeeRepository;
        private readonly string _reportExpected = "Январь 2011\r\n--------------------------------------------\r\nIT\r\nJohn0 100р\r\nJohn1 200р\r\nJohn2 300р\r\nВсего по отделу: 600р\r\n--------------------------------------------\r\nHR\r\nJohn4 400р\r\nJohn5 500р\r\nВсего по отделу: 900р\r\n--------------------------------------------\r\nQA\r\nJohn6 0р\r\nВсего по отделу: 0р\r\n--------------------------------------------\r\nВсего по предприятию: 1500р\r\n";

        [SetUp]
        public void SetUp()
        {
            var departmentRepositoryMock = new Mock<IDepartmentRepository>();

            departmentRepositoryMock.Setup(x => x.GetDepartments())
                .Returns(new List<Department>
                {
                    new Department { Id = 0, Name = "IT" },
                    new Department { Id = 1, Name = "HR" },
                    new Department { Id = 2, Name = "QA" }
                });
            _departmentRepository = departmentRepositoryMock.Object;

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            employeeRepositoryMock.Setup(x => x.GetEmployeesByDepartmentId(It.Is<int>(x => x == 0)))
                .Returns(Task.FromResult(new List<Employee>
                    {
                        new Employee { BuhCode = "", Inn = "1", Name = "John0", Salary = 100},
                        new Employee { BuhCode = "", Inn = "2", Name = "John1", Salary = 200},
                        new Employee { BuhCode = "", Inn = "3", Name = "John2", Salary = 300},
                    }));
            employeeRepositoryMock.Setup(x => x.GetEmployeesByDepartmentId(It.Is<int>(x => x == 1)))
                .Returns(Task.FromResult(new List<Employee>
                    {
                        new Employee { BuhCode = "", Inn = "4", Name = "John4", Salary = 400},
                        new Employee { BuhCode = "", Inn = "6", Name = "John5", Salary = 500},
                    }));
            employeeRepositoryMock.Setup(x => x.GetEmployeesByDepartmentId(It.Is<int>(x => x == 2)))
                .Returns(Task.FromResult(new List<Employee>
                    {
                        new Employee { BuhCode = "", Inn = "7", Name = "John6", Salary = 0},
                    }));

            _employeeRepository = employeeRepositoryMock.Object;
        }

        [Test]
        public async Task ReportContentIsCorrect()
        {
            var reportBuilder = new EmployeeReportBuilder(_employeeRepository, _departmentRepository);

            var report = await reportBuilder.BuildAsync(2011, 1);

            Assert.IsTrue(_reportExpected == report);
        }
    }
}
