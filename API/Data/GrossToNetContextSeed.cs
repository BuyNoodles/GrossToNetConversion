using API.Entities;
using System.Text.Json;

namespace API.Data
{
    public class GrossToNetContextSeed
    {
        public static async Task SeedAsync(GrossToNetContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Employees.Any())
                {
                    var usersData =
                        File.ReadAllText("Data/SeedData/EmployeesSeed.json");

                    var employees = JsonSerializer.Deserialize<List<Employee>>(usersData);

                    foreach (var item in employees)
                    {
                        context.Employees.Add(item);
                    }

                    await context.SaveChangesAsync();

                    foreach (var item in employees)
                    {
                        var Tax = 0.1m * item.GrossIncome;
                        var PIO = 0.14m * item.GrossIncome;
                        var HealthCare = 0.0515m * item.GrossIncome;
                        var Unemployment = 0.0075m * item.GrossIncome;

                        context.IncomeDetails.Add(new IncomeDetails
                        {
                            EmployeeId = item.Id,
                            Tax = Tax,
                            PIO = PIO,
                            HealthCare = HealthCare,
                            Unemployment = Unemployment,
                            NetIncome = item.GrossIncome - Tax - PIO - HealthCare - Unemployment
                        });
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<GrossToNetContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
