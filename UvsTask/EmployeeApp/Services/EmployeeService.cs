using EmployeeApp.Data;
using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Services;

public class EmployeeService
{
    private readonly EmployeeContext _context;

    public EmployeeService(EmployeeContext context)
    {
        _context = context;
    }

    public async Task SetEmployeeAsync(int id, string name, int salary)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            employee = new EmployeeApp.Models.Employee
            {
                EmployeeId = id,
                EmployeeName = name,
                EmployeeSalary = salary
            };
            _context.Employees.Add(employee);
        }
        else
        {
            employee.EmployeeName = name;
            employee.EmployeeSalary = salary;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<EmployeeApp.Models.Employee?> GetEmployeeAsync(int id)
    {
        return await _context.Employees.FindAsync(id);
    }
}
