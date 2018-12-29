using Microsoft.EntityFrameworkCore;

namespace FestoVideoStream.Data
{
    public class DevicesContext : DbContext
    {
        public DevicesContext (DbContextOptions<DevicesContext> options)
            : base(options)
        {
        }

        public DbSet<FestoVideoStream.Models.Device> Devices { get; set; }
    }
}
