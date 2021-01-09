using System.Collections.Generic;
using ReportService.Models;

namespace ReportService.Abstract
{
    public interface IDepartmentRepository
    {
        List<Department> GetDepartments();
    }
}
