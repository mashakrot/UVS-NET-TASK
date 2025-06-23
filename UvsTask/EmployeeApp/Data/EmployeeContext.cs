using Microsoft.EntityFrameworkCore;
using EmployeeApp.Models;

namespace EmployeeApp.Data;

public class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options) { }

    public DbSet<EmployeeApp.Models.Employee> Employees { get; set; } = null!;
}
