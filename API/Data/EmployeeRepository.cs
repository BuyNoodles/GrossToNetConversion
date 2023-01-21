using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PdfSharp.Pdf;
using PdfSharp;
using System.Text;
using System.Text.Json;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using AutoMapper;

namespace API.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly GrossToNetContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly IMapper _mapper;

        public EmployeeRepository(GrossToNetContext context, IConfiguration config, 
            ILogger<EmployeeRepository> logger, IMapper mapper)
        {
            _context = context;
            _config = config;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            // Calculate Tax, PIO.. from gross income
            IncomeDetails details = CalculateIncome(employee.GrossIncome);

            details.EmployeeId = employee.Id;

            _context.IncomeDetails.Add(details);

            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null) return false;

            _context.Remove(employee);

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _context.Employees
                .Include(p => p.IncomeDetails)
                .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        public async Task<DetailedEmployeeDto> GetEmployeeByIdAsync(int id, string currency = "rsd")
        {
            var query = await _context.Employees
                .Include(p => p.IncomeDetails)
                .FirstOrDefaultAsync(x => x.Id == id);

            var employee = _mapper.Map<Employee, DetailedEmployeeDto>(query);

            // Convert to EUR or USD using public API
            if(employee != null && currency != "rsd")
            {
                var convertedGross = await Convert(employee.GrossIncome, "RSD", currency.ToUpper());

                var newIncome = CalculateIncome(convertedGross);

                employee.GrossIncome = convertedGross;
                employee.IncomeDetails.Tax = newIncome.Tax;
                employee.IncomeDetails.PIO = newIncome.PIO;
                employee.IncomeDetails.HealthCare = newIncome.HealthCare;
                employee.IncomeDetails.Unemployment = newIncome.Unemployment;
                employee.IncomeDetails.NetIncome = convertedGross -
                    newIncome.Tax - newIncome.PIO - newIncome.HealthCare - newIncome.Unemployment;
            }

            return employee;
        }

        public async Task<bool> ExportToExcelAsync()
        {
            var employees = await GetAllEmployeesAsync();

            if (!employees.Any()) return false;

            ExcelPackage excel = new ExcelPackage();

            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Employees");

            worksheet.Cells[1, 1].LoadFromCollection(employees, true);

            await excel.SaveAsAsync("Content/Files/Employees.xlsx");

            return true;
        }

        public async Task<bool> ExportToCsvAsync()
        {
            var employees = await GetAllEmployeesAsync();

            if (!employees.Any()) return false;

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

        public async Task<bool> ExportToPdfAsync(int id)
        {
            var employee = await GetEmployeeByIdAsync(id);

            if (employee == null) return false;

            var html = HtmlFormat.Format(employee);

            PdfDocument pdf = PdfGenerator.GeneratePdf(html, PageSize.A4);
            pdf.Save("Content/Files/Employee.pdf");

            return true;
        }

        public async Task<decimal> Convert(decimal amount, string from, string to)
        {
            HttpClient client = new HttpClient();

            string apiKey = _config["ApiCredentials:ExchangeRatesKey"];

            var message = new HttpRequestMessage(HttpMethod.Get,
                "https://api.apilayer.com/exchangerates_data/convert" +
                $"&from={from}&to={to}&amount={(int)amount}");

            message.Headers.Add("apikey", apiKey);

            var response = await client.SendAsync(message);
            string content = await response.Content.ReadAsStringAsync();

            // _logger.LogInformation(content);

            ConversionResult result = JsonSerializer.Deserialize<ConversionResult>(content);

            return result.result;
        }

        public IncomeDetails CalculateIncome(decimal grossIncome)
        {
            // Calculate Net income from Gross income
            var tax = 0.1m * grossIncome;
            var pio = 0.14m * grossIncome;
            var healthCare = 0.0515m * grossIncome;
            var unemployment = 0.0075m * grossIncome;

            return new IncomeDetails
            {
                Tax = tax,
                PIO = pio,
                HealthCare = healthCare,
                Unemployment = unemployment,
                NetIncome = grossIncome - tax - pio - healthCare - unemployment
            };
        }
    }
}
