using Microsoft.EntityFrameworkCore;
using Employee.Models;

namespace Employee.Data
{
    public class EmployeeContext : DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();

        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }
    }
}
