using API.Entities;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
