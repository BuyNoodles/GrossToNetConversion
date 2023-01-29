using Microsoft.AspNetCore.Components;
using Models.Dtos;
using View.Interfaces;

namespace View.Pages
{
    public class EmployeeCreateBase : ComponentBase
    {
        [Inject]
        public IEmployeeService EmployeeService{ get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public EmployeeDto Employee { get; set; } = new EmployeeDto();
        public string ErrorMessage { get; set; }

        public async Task OnSubmit(EmployeeDto employee)
        {
            try
            {
                await EmployeeService.CreateEmployee(employee);
                NavigationManager.NavigateTo("/?create=true");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


    }
}
