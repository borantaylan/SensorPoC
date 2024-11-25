using Microsoft.EntityFrameworkCore;
using SensorPoC.Domain.Contracts;

namespace SensorPoC.Storage
{
    internal class SensorDbContext : DbContext, ISensorDbContext
    {
        public SensorDbContext(DbContextOptions<SensorDbContext> options) : base(options) { }

        public DbSet<Sensor> Sensors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sensor>()
                .Property(x=>x.Identity)
                .ValueGeneratedNever();
            modelBuilder.Entity<Sensor>()
                .HasKey(x => x.Identity);
        }
    }
}
