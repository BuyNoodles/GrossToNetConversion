using Models.Dtos;

namespace View.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetEmployees();
        Task<DetailedEmployeeDto> GetEmployee(int id, string currency);
        Task<bool> DeleteEmployee(int id);
        Task<bool> SendPdfToEmployee(int id);
    }
}
