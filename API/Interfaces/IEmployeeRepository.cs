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
        bool ExportToPdf(DetailedEmployeeDto employee);
        Task<decimal> ConvertCurrency(decimal amount, string from, string to);
        IncomeDetails CalculateIncome(decimal grossIncome);
        Task<bool> SendPdfToEmail(DetailedEmployeeDto employee);
    }
}
