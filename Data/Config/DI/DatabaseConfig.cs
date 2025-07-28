using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data.DI;

public static class DatabaseConfig
{
    public static IServiceCollection ConfigureDb(this IServiceCollection services)
    {
        services.AddScoped<IFlyghtzDbContext, FlyghtDbContext>();
        services.AddDbContext<FlyghtDbContext>();

        // Apply migrations
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<FlyghtDbContext>();
            db.Database.Migrate();
        }

        return services;
    }
}