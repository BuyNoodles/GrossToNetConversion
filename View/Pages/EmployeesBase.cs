using Microsoft.AspNetCore.Components;
using Models.Dtos;
using View.Interfaces;

namespace View.Pages
{
    public class EmployeesBase : ComponentBase
    {
        [Inject]
        public IEmployeeService EmployeeService { get; set; }
        [Inject]
        public HttpClient HttpClient { get; set; }
        [Parameter]
        [SupplyParameterFromQuery(Name = "create")]
        public bool CreateSuccess { get; set; }
        [Parameter]
        [SupplyParameterFromQuery(Name = "delete")]
        public bool DeleteSuccess { get; set; }
        public List<EmployeeDto> Employees { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Employees = await EmployeeService.GetEmployees();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;

            }
        }
    }
}
