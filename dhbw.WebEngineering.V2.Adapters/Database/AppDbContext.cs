using dhbw.WebEngineering.V2.Domain.Building;
using dhbw.WebEngineering.V2.Domain.Room;
using dhbw.WebEngineering.V2.Domain.Storey;
using Microsoft.EntityFrameworkCore;

namespace dhbw.WebEngineering.V2.Adapters.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Building> buildings { get; set; }
    public DbSet<Storey> storeys { get; set; }
    public DbSet<Room> rooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Building>().HasQueryFilter(b => b.deleted_at == null);
        modelBuilder.Entity<Storey>().HasQueryFilter(s => s.deleted_at == null);
        modelBuilder.Entity<Room>().HasQueryFilter(r => r.deleted_at == null);

        base.OnModelCreating(modelBuilder);
    }
}
