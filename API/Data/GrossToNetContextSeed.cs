using API.Entities;
using System.Text.Json;

namespace API.Models
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
