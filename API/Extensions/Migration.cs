using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class Migration
    {
        public static async Task Migrate(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<GrossToNetContext>();
                    await context.Database.MigrateAsync();
                    await GrossToNetContextSeed.SeedAsync(context, loggerFactory);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occured during migration");
                }
            }
        }
    }
}
