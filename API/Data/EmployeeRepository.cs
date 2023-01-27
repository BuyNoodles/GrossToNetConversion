using Models.Dtos;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Text;
using System.Text.Json;
using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SendGrid;
using SendGrid.Helpers.Mail;

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
                var convertedGross = await ConvertCurrency(employee.GrossIncome, "RSD", currency.ToUpper());

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

            sb.AppendLine("Id;FirstName;LastName;Address;Email;GrossIncome;WorkPosition");

            foreach(var employee in employees)
            {
                sb.AppendLine($"{employee.Id};{employee.FirstName};{employee.LastName};" +
                    $"{employee.Address};{employee.Email};{employee.GrossIncome};{employee.WorkPosition}");
            }

            await File.WriteAllTextAsync("Content/Files/Employees.csv", sb.ToString());

            return true;
        }

        public bool ExportToPdf(DetailedEmployeeDto employee)
        {
            var pdf = new Document(iTextSharp.text.PageSize.A4);
            var midFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 20);
            var smallFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 17);
            
            PdfWriter.GetInstance(pdf, 
                new FileStream($"Content/Files/{employee.FirstName}{employee.LastName}_Employee.pdf", 
                FileMode.Create));           
            pdf.Open();

            pdf.Add(new Paragraph($"{employee.FirstName} {employee.LastName}", midFont));
            pdf.Add(new Paragraph($"{employee.Address}", smallFont));
            pdf.Add(new Paragraph($"{employee.Email}", smallFont));
            pdf.Add(new Paragraph($"{employee.WorkPosition}", smallFont));

            var table = new PdfPTable(2);
            table.SpacingBefore = 20f;

            table.AddCell("Gross Income");
            table.AddCell($"{employee.GrossIncome}");
            table.AddCell("Tax");
            table.AddCell($"{employee.IncomeDetails.Tax}");
            table.AddCell("Pension/PIO");
            table.AddCell($"{employee.IncomeDetails.PIO}");
            table.AddCell("Health Care");
            table.AddCell($"{employee.IncomeDetails.HealthCare}");
            table.AddCell("Unemployment");
            table.AddCell($"{employee.IncomeDetails.Unemployment}");
            table.AddCell("Net Income");
            table.AddCell($"{employee.IncomeDetails.NetIncome}");

            pdf.Add(table);

            pdf.Close();

            return true;
        }

        public async Task<bool> SendPdfToEmail(DetailedEmployeeDto employee)
        {
            string apiKey = _config["ApiCredentials:SendGrid:ApiKey"];
            string senderEmail = _config["ApiCredentials:SendGrid:SenderEmail"];

            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(senderEmail, "GrossToNetConversion");
            var to = new EmailAddress(employee.Email, $"{employee.FirstName} {employee.LastName}");
            var msg = MailHelper.CreateSingleEmail(from, to, "Forwarded PDF Document", 
                "Hello,\n\nYou'll find the attached PDF file below.\n\nGrossToNetConversion",
                "Hello,<br><br>You'll find the attached PDF file below.<br><br>Warm Regards<br>GrossToNetConversion");

            var file = File.ReadAllBytes($"Content/Files/{employee.FirstName}{employee.LastName}_Employee.pdf");

            msg.AddAttachment("employee_report.pdf", Convert.ToBase64String(file));
            var response = await client.SendEmailAsync(msg);

            // _logger.LogInformation(await response.Body.ReadAsStringAsync());

            if (!response.IsSuccessStatusCode) return false;

            return true;
        }

        public async Task<decimal> ConvertCurrency(decimal amount, string from, string to)
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
