using FestoVideoStream.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Device = FestoVideoStream.Models.Entities.Device;

namespace FestoVideoStream.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(user => new {user.Id, user.Login});
            modelBuilder.Entity<User>().Property("Id").ValueGeneratedOnAdd();
        }
    }
}
