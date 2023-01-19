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
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            return Ok(await _employeeRepository.GetEmployeeByIdAsync(id));
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddEmployee([FromBody] Employee employee)
        {
            var result = await _employeeRepository.AddEmployeeAsync(employee);

            if (result) return Ok();

            return BadRequest("Failed to add employee");
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
    }
}
