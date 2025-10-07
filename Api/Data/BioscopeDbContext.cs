using EFCore.NamingConventions;
using Microsoft.EntityFrameworkCore;
using PlantAnimalApi.Models; 

namespace PlantAnimalApi.Data
{
    public class BioscopeDbContext : DbContext
    {
        public BioscopeDbContext(DbContextOptions<BioscopeDbContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Discovery> Discoveries => Set<Discovery>();
        public DbSet<MediaAsset> MediaAssets => Set<MediaAsset>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Reaction> Reactions => Set<Reaction>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.UseSnakeCaseNamingConvention();

            b.Entity<User>(e =>
            {
                e.ToTable("users");
                e.HasKey(x => x.Id);
                e.Property(x => x.Email).IsRequired();
                e.Property(x => x.PasswordHash).IsRequired();
                e.Property(x => x.DisplayName).IsRequired();
                e.HasIndex(x => x.CreatedAt);
            });

            b.Entity<Discovery>(e =>
            {
                e.ToTable("discoveries");
                e.HasKey(x => x.Id);
                e.Property(x => x.Confidence).HasColumnType("numeric(5,4)");
                e.Property(x => x.ApiResult).HasColumnType("jsonb");
                e.HasIndex(x => new { x.UserId, x.CreatedAt });
                e.HasIndex(x => new { x.IsPublic, x.CreatedAt });
                e.HasIndex(x => x.TopMatchName);
                e.HasIndex(x => new { x.Latitude, x.Longitude });
                e.HasOne(x => x.User).WithMany(u => u.Discoveries).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            b.Entity<MediaAsset>(e =>
            {
                e.ToTable("media_assets");
                e.HasKey(x => x.Id);
                e.Property(x => x.Url).IsRequired();
                e.HasIndex(x => x.DiscoveryId);
                e.HasOne(x => x.Discovery).WithMany(d => d.MediaAssets).HasForeignKey(x => x.DiscoveryId).OnDelete(DeleteBehavior.Cascade);
            });

            b.Entity<Comment>(e =>
            {
                e.ToTable("comments");
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.DiscoveryId, x.CreatedAt });
                e.HasIndex(x => new { x.UserId, x.CreatedAt });
                e.HasOne(x => x.Discovery).WithMany(d => d.Comments).HasForeignKey(x => x.DiscoveryId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            b.Entity<Reaction>(e =>
            {
                e.ToTable("reactions");
                e.HasKey(x => new { x.DiscoveryId, x.UserId });
                e.HasOne(x => x.Discovery).WithMany(d => d.Reactions).HasForeignKey(x => x.DiscoveryId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
