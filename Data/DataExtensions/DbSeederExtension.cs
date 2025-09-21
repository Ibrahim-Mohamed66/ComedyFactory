using Data.DBInitializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Data.Extensions.DataExtensions
{
    public static class DbSeederExtension
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            try
            {
                await dbInitializer.Initialize();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<IDbInitializer>>();
                logger.LogError(ex, "Database seeding failed.");
            }
        }
    }
}
