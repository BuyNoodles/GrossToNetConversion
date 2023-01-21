using API.Dtos;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<DetailedEmployeeDto> GetEmployeeByIdAsync(int id, string currency);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<bool> ExportToExcelAsync();
        Task<bool> ExportToCsvAsync();
        Task<bool> ExportToPdfAsync(int id);
        Task<decimal> Convert(decimal amount, string from, string to);
        IncomeDetails CalculateIncome(decimal grossIncome);
    }
}
