using Microsoft.AspNetCore.Components;
using Models.Dtos;
using View.Interfaces;

namespace View.Pages
{
    public class EmployeeDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Parameter]
        [SupplyParameterFromQuery(Name = "currency")]
        public string Currency { get; set; }
        [Inject]
        public IEmployeeService EmployeeService{ get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public HttpClient HttpClient { get; set; }
        public DetailedEmployeeDto Employee{ get; set; }
        public string ErrorMessage { get; set; }
        public bool Notification { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Employee = await EmployeeService.GetEmployee(Id, Currency);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                
            }
        }

        protected void ConvertCurrency_Click(int id, string currency)
        {
            try
            {
                NavigationManager.NavigateTo($"/employees/{Employee.Id}?currency={currency}", forceLoad: true);
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async Task DeleteEmployee_Click(int id)
        {
            try
            {
                await EmployeeService.DeleteEmployee(id);
                NavigationManager.NavigateTo("/?delete=true");
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async Task SendPdfToEmployee_Click(int id)
        {
            try
            {
                await EmployeeService.SendPdfToEmployee(id);
                Notification = true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
