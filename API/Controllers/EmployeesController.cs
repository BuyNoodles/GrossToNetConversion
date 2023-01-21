using API.Entities;
using API.Errors;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EmployeesController : BaseApiController
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeRepository employeeRepository, ILogger<EmployeesController> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            if (!employees.Any()) return NotFound(new ApiResponse(404,
                $"No employees found.."));

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id, string currency = "rsd")
        {
            currency = currency.ToLower();

            if (currency != "rsd" && currency != "eur" & currency != "usd")
                return BadRequest(new ApiResponse(400, 
                    "Currency can only be RSD, EUR or USD.."));

            var employee = await _employeeRepository.GetEmployeeByIdAsync(id, currency);
            if (employee == null) return NotFound(new ApiResponse(404,
                $"Employee with Id {id} not found"));

            return Ok(employee);
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddEmployee([FromBody] Employee employee)
        {
            var result = await _employeeRepository.AddEmployeeAsync(employee);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeRepository.DeleteEmployeeAsync(id);

            if (result) return NoContent();

            return BadRequest(new ApiResponse(400, $"No employee with Id {id} found"));
        }

        [HttpGet("export/excel")]
        public async Task<ActionResult> ExportToExcel()
        {
            var response = await _employeeRepository.ExportToExcelAsync();

            if (!response) return NotFound(new ApiResponse(404,
                $"No employees found.."));

            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "Content/Files/Employees.xlsx"), 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Employees.xlsx");
        }
        
        [HttpGet("export/csv")]
        public async Task<ActionResult> ExportToCsv()
        {
            var response = await _employeeRepository.ExportToCsvAsync();

            if (!response) return NotFound(new ApiResponse(404,
                $"No employees found.."));

            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "Content/Files/Employees.csv"),
                "text/csv",
                "Employees.csv");
        }

        [HttpGet("export/pdf/{id}")]
        public async Task<ActionResult> ExportToPdf(int id)
        {
            var response = await _employeeRepository.ExportToPdfAsync(id);

            if (!response) return NotFound(new ApiResponse(404,
                $"Employee with Id {id} not found"));

            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "Content/Files/Employee.pdf"),
                "application/pdf",
                "Employee.pdf");
        }
    }
}
