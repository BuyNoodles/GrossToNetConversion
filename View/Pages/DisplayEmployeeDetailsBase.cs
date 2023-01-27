using Microsoft.AspNetCore.Components;
using Models.Dtos;

namespace View.Pages
{
    public class DisplayEmployeeDetailsBase : ComponentBase
    {
        [Parameter]
        public DetailedEmployeeDto Employee { get; set; }
    }
}
