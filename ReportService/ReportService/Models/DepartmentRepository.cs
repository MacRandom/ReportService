using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using ReportService.Abstract;

namespace ReportService.Models
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString;

        public DepartmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:Default"];
        }

        public List<Department> GetDepartments()
        {
            var departments = new List<Department>();

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    departments = db.Query<Department>("SELECT d.id, d.name from deps d where d.active = true").ToList();
                }
            }
            catch (Exception exc)
            {
                throw new Exception("Ошибка подключения к базе данных", exc);
            }

            return departments;
        }
    }
}
