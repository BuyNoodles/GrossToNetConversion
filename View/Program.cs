using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using View;
using View.Interfaces;
using View.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var services = builder.Services;
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri("https://localhost:5001")
});

services.AddScoped<IEmployeeService, EmployeeService>();

services.AddScoped<DialogService>();
services.AddScoped<NotificationService>();
services.AddScoped<TooltipService>();
services.AddScoped<ContextMenuService>();

await builder.Build().RunAsync();
