using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ushio.Infrastructure.Database
{
    public class UshioContextFactory : IDesignTimeDbContextFactory<UshioDbContext>
    {
        public UshioDbContext CreateDbContext(string[] args)
        {
            var opts = new DbContextOptionsBuilder<UshioDbContext>();
            var cfg = new ConfigurationBuilder().AddUserSecrets<UshioDbContext>().Build();

            opts.UseNpgsql(cfg.GetConnectionString("Database"));

            return new UshioDbContext(opts.Options);
        }
    }
}
