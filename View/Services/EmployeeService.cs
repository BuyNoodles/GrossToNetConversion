using Models.Dtos;
using System.Net.Http.Json;
using System.Text;
using View.Interfaces;

namespace View.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(HttpClient httpClient, ILogger<EmployeeService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> CreateEmployee(EmployeeDto employee)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/employees/add", employee);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var message = await response.Content.ReadFromJsonAsync<Error>();
                    throw new Exception(message.Message + ", all fields must be filled");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<DetailedEmployeeDto> GetEmployee(int id, string currency)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/employees/{id}?currency={currency}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        return default(DetailedEmployeeDto);

                    return await response.Content.ReadFromJsonAsync<DetailedEmployeeDto>();
                }
                else
                {
                    var message = await response.Content.ReadFromJsonAsync<Error>();
                    throw new Exception(message.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            } 
        }

        public async Task<List<EmployeeDto>> GetEmployees()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/employees");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        return new List<EmployeeDto>();

                    return await response.Content.ReadFromJsonAsync<List<EmployeeDto>>();
                }
                else
                {
                    var message = await response.Content.ReadFromJsonAsync<Error>();
                    throw new Exception(message.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<bool> SendPdfToEmployee(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/employees/export/pdf/{id}/forward");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var message = await response.Content.ReadFromJsonAsync<Error>();
                    throw new Exception(message.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/employees/delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var message = await response.Content.ReadFromJsonAsync<Error>();
                    throw new Exception(message.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }

    public class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
