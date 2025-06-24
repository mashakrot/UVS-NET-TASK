using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EmployeeApp.Data;
using EmployeeApp.Services;
using DotNetEnv;


namespace EmployeeApp
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Env.Load("../../UvsTask/.env");

            var DbHost = Environment.GetEnvironmentVariable("UvsTaskHost")
            ?? throw new InvalidOperationException("You must set the UvsTaskHost environment variable");
            var  DbPort = Environment.GetEnvironmentVariable("UvsTaskPort") 
            ?? throw new InvalidOperationException("You must set the UvsTaskPort environment variable");
            var DbUser = Environment.GetEnvironmentVariable("UvsTaskUser") 
            ?? throw new InvalidOperationException("You must set the UvsTaskUser environment variable");
            var DbPassword = Environment.GetEnvironmentVariable("UvsTaskPassword") 
            ?? throw new InvalidOperationException("You must set the UvsTaskPassword environment variable");
            var DbName = Environment.GetEnvironmentVariable("UvsTaskDatabase") 
            ?? throw new InvalidOperationException("You must set the UvsTaskDatabase environment variable");

            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    Console.WriteLine($"Host: {DbHost}, Port: {DbPort}, User: {DbUser}, DB: {DbName}");

                    logging.ClearProviders();
                    logging.AddConsole();

                    // if (OperatingSystem.IsWindows())
                    // {
                    //     logging.AddEventLog();
                    // }
                })
                .ConfigureServices((context, services) =>
                {
                    var connectionString = $"Host={DbHost};Port={DbPort};Username={DbUser};Password={DbPassword};Database={DbName}";

                    services.AddDbContext<EmployeeContext>(options =>
                        options.UseNpgsql(connectionString));

                    services.AddScoped<EmployeeService>();
                });

            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<EmployeeService>();

            // Parse CLI arguments
            var command = args.Length > 0 ? args[0] : null;

            try
            {
                if (command == "set-employee")
                {
                    var id = int.Parse(GetArg(args, "--employeeId"));
                    var name = GetArg(args, "--employeeName");
                    var salary = int.Parse(GetArg(args, "--employeeSalary"));

                    await service.SetEmployeeAsync(id, name, salary);
                    Console.WriteLine("Employee saved.");
                }
                else if (command == "get-employee")
                {
                    var id = int.Parse(GetArg(args, "--employeeId"));
                    var employee = await service.GetEmployeeAsync(id);
                    if (employee == null)
                        Console.WriteLine("Employee not found.");
                    else
                        Console.WriteLine($"ID: {employee.EmployeeId}, Name: {employee.EmployeeName}, Salary: {employee.EmployeeSalary}");
                }
                else
                {
                    Console.WriteLine("Invalid command. Use get-employee or set-employee.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            static string GetArg(string[] args, string name)
            {
                var index = Array.IndexOf(args, name);
                if (index == -1 || index == args.Length - 1)
                    throw new ArgumentException($"Missing value for {name}");
                return args[index + 1];
            }
        }
    }
}
