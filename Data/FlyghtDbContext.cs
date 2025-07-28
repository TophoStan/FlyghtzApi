using Data.Contracts;
using Environment;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class FlyghtDbContext : DbContext, IFlyghtzDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseNpgsql(EnvironmentExtension.GetPostgresConnectionString(),
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly("Data");
                        npgsqlOptions.CommandTimeout(30);
                    })
                .EnableThreadSafetyChecks(false) // Disable safety checks as we'll handle concurrency
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IEntityTypeConfiguration<>).Assembly);
    }

    public DbSet<AirportEntity> AirportEntities { get; set; }
}