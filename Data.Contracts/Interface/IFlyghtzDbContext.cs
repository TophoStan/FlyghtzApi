using Microsoft.EntityFrameworkCore;

namespace Data.Contracts;

public interface IFlyghtzDbContext
{
    public DbSet<AirportEntity> AirportEntities { get; set; }
}