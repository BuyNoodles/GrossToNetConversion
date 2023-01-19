using API.Entities;
using API.Interfaces;
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
            var newEmployee = _context.Employees.Add(employee);

            var employeeResult = await _context.SaveChangesAsync();

            if(employeeResult < 0) return false;

            var Tax = 0.1m * employee.GrossIncome;
            var PIO = 0.14m * employee.GrossIncome;
            var HealthCare = 0.0515m * employee.GrossIncome;
            var Unemployment = 0.0075m * employee.GrossIncome;

            IncomeDetails details = new IncomeDetails
            {
                EmployeeId = employee.Id,
                Tax = Tax,
                PIO = PIO,
                HealthCare = HealthCare,
                Unemployment = Unemployment,
                NetIncome = employee.GrossIncome - Tax - PIO - HealthCare - Unemployment
            };

            _context.IncomeDetails.Add(details);

            var detailsResult = await _context.SaveChangesAsync();

            return detailsResult > 0;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(p => p.IncomeDetails)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(p => p.IncomeDetails)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExportToExcelAsync()
        {
            var employees = await GetAllEmployeesAsync();

            ExcelPackage excel = new ExcelPackage();

            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Employees");

            worksheet.Cells[1, 1].LoadFromCollection(employees, true);

            await excel.SaveAsAsync("Content/Files/Employees.xlsx");

            return true;
        }

        public async Task<bool> ExportToCsvAsync()
        {
            var employees = await GetAllEmployeesAsync();

            var sb = new StringBuilder();

            sb.AppendLine("Id;FirstName;LastName;Address;GrossIncome;WorkPosition");

            foreach(var employee in employees)
            {
                sb.AppendLine($"{employee.Id};{employee.FirstName};{employee.LastName};" +
                    $"{employee.Address};{employee.GrossIncome};{employee.WorkPosition}");
            }

            await File.WriteAllTextAsync("Content/Files/Employees.csv", sb.ToString());

            return true;
        }
    }
}
