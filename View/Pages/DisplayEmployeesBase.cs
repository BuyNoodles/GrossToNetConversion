using Microsoft.AspNetCore.Components;
using Models.Dtos;

namespace View.Pages
{
    public class DisplayEmployeesBase : ComponentBase
    {
        [Parameter]
        public List<EmployeeDto> Employees { get; set; }
        [Parameter]
        public string Currency { get; set; }
    }
}
