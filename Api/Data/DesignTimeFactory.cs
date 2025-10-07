using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PlantAnimalApi.Data
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<BioscopeDbContext>
    {
        public BioscopeDbContext CreateDbContext(string[] args)
        {
            // Use your dev connection string here (safe – used at design-time only)
            var cs = Environment.GetEnvironmentVariable("DATABASE_URL")
                     ?? "Host=localhost;Port=5432;Database=bioscope;Username=bioscope_app;Password=YOUR_PASSWORD;Ssl Mode=Require;Trust Server Certificate=true";

            // If DATABASE_URL style is used, convert it
            if (cs.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase))
            {
                var u = new Uri(cs); var p = u.UserInfo.Split(':');
                cs = $"Host={u.Host};Port={u.Port};Database={u.AbsolutePath.Trim('/')};Username={Uri.UnescapeDataString(p[0])};Password={Uri.UnescapeDataString(p[1])};Ssl Mode=Require;Trust Server Certificate=true;Pooling=true;";
            }

            var opt = new DbContextOptionsBuilder<BioscopeDbContext>()
                .UseNpgsql(cs)
                .UseSnakeCaseNamingConvention()
                .Options;

            return new BioscopeDbContext(opt);
        }
    }
}
