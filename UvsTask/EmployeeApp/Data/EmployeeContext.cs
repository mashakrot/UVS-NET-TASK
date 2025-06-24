using Microsoft.EntityFrameworkCore;
using EmployeeApp.Models;

namespace EmployeeApp.Data;

public class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options) { }

    public DbSet<EmployeeApp.Models.Employee> Employees { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees"); // Match lowercase table name

            entity.Property(e => e.EmployeeId).HasColumnName("employeeid");
            entity.Property(e => e.EmployeeName).HasColumnName("employeename");
            entity.Property(e => e.EmployeeSalary).HasColumnName("employeesalary");
        });
    }
}
