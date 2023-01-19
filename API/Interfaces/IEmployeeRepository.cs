﻿using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<bool> AddEmployeeAsync(Employee employee);
        Task<bool> ExportToExcel();
        Task<bool> ExportToCsv();
    }
}
