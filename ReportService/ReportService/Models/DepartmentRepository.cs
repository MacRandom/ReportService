using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using ReportService.Abstract;

namespace ReportService.Models
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString = "Host=192.168.99.100;Username=postgres;Password=1;Database=employee";

        public List<Department> GetDepartments()
        {
            var departments = new List<Department>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                departments = db.Query<Department>("SELECT d.id, d.name from deps d where d.active = true").ToList();
            }

            return departments;
        }
    }
}
