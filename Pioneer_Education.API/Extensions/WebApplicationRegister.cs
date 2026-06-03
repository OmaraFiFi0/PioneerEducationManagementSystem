using Microsoft.EntityFrameworkCore;
using Pioneer_Education.Infrastructure.Data.Contexts;

namespace Pioneer_Education.API.Extensions
{
    public static class WebApplicationRegister
    {
        public static async Task<WebApplication> MigrateDataBaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var pendingMigrations = await applicationDbContext.Database.GetPendingMigrationsAsync();


            if (pendingMigrations.Any())
                await applicationDbContext.Database.MigrateAsync();

            return app;
        }
    }
}
