using FestoVideoStream.Entities;
using Microsoft.EntityFrameworkCore;

namespace FestoVideoStream.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<DevicesContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
