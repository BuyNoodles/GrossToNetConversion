using API.Entities;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Text;

namespace API.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly GrossToNetContext _context;

        public EmployeeRepository(GrossToNetContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            var employeeResult = await _context.SaveChangesAsync();

            return employeeResult > 0;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<bool> ExportToExcel()
        {
            var employees = await GetAllEmployeesAsync();

            ExcelPackage excel = new ExcelPackage();

            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Employees");

            worksheet.Cells[1, 1].LoadFromCollection(employees, true);

            await excel.SaveAsAsync("Content/Files/Employees.xlsx");

            return true;
        }

        public async Task<bool> ExportToCsv()
        {
            var employees = await GetAllEmployeesAsync();

            var sb = new StringBuilder();

            sb.AppendLine("Id;FirstName;LastName;Address;GrossIncome;WorkPosition");

            foreach(var employee in employees)
            {
                sb.AppendLine($"{employee.Id};{employee.FirstName};{employee.LastName};" +
                    $"{employee.Address};{employee.GrossIncome};{employee.WorkPosition}");
            }

            File.WriteAllText("Content/Files/Employees.csv", sb.ToString());

            return true;
        }
    }
}
