using FestoVideoStream.Entities;
using Microsoft.EntityFrameworkCore;

namespace FestoVideoStream.Data
{
    public class DevicesContext : DbContext
    {
        public DevicesContext (DbContextOptions<DevicesContext> options)
            : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForNpgsqlUseIdentityAlwaysColumns();
        }
    }
}
