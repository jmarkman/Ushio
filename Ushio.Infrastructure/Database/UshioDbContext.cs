using Microsoft.EntityFrameworkCore;
using Npgsql;
using Ushio.Infrastructure.Database.Data.Models;

namespace Ushio.Infrastructure.Database
{
    public class UshioDbContext : DbContext
    {
        public DbSet<VideoClip> VideoClips { get; set; }

        public UshioDbContext() : base()
        {

        }

        public UshioDbContext(DbContextOptions<UshioDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasPostgresExtension("uuid-ossp");
        }
    }
}
