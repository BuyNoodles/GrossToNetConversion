using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EmployeesController : BaseApiController
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployees()
        {
            return Ok(await _employeeRepository.GetAllEmployeesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id, string currency = "RSD")
        {   
            return Ok(await _employeeRepository.GetEmployeeByIdAsync(id, currency));
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddEmployee([FromBody] Employee employee)
        {
            var result = await _employeeRepository.AddEmployeeAsync(employee);

            if (result) return Ok("Employee Added");

            return BadRequest();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeRepository.DeleteEmployeeAsync(id);

            if (result) return Ok("Employee Deleted");

            return BadRequest();

        }

        [HttpGet("export/excel")]
        public async Task<ActionResult> ExportToExcel()
        {
            await _employeeRepository.ExportToExcelAsync();

            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "Content/Files/Employees.xlsx"), 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Employees.xlsx");
        }
        
        [HttpGet("export/csv")]
        public async Task<ActionResult> ExportToCsv()
        {
            await _employeeRepository.ExportToCsvAsync();

            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "Content/Files/Employees.csv"),
                "text/csv",
                "Employees.csv");
        }

        [HttpGet("export/pdf/{id}")]
        public async Task<ActionResult> ExportToPdf(int id)
        {
            await _employeeRepository.ExportToPdfAsync(id);

            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "Content/Files/Employee.pdf"),
                "application/pdf",
                "Employee.pdf");
        }
    }
}
