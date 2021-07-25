using Microsoft.EntityFrameworkCore;
using Ushio.Data.DatabaseModels;

namespace Ushio.Infrastructure.Database
{
    public class UshioDbContext : DbContext
    {
        public DbSet<VideoClip> VideoClips { get; set; }
        public DbSet<FightingGameVod> FightingGameVods { get; set; }

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
