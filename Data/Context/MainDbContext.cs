using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;



namespace Data.Context
{
    public abstract class MainDbContext:IdentityDbContext<ApplicationUser>
    {

        public MainDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Desire> Desires { get; set; }
        public DbSet<MasterCategory> MasterCategories { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<PersonalData> PersonalDatas { get; set; }

        public DbSet<Album> Albums { get; set; }
        public DbSet<AlbumMedia> AlbumMedias { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);
            builder.Entity<Country>()
                 .Property(c => c.CreatedOnUtc)
                 .HasDefaultValueSql("NOW()");

            builder.Entity<City>()
                .Property(c => c.CreatedOnUtc)
                .HasDefaultValueSql("NOW()");

            builder.Entity<Configuration>()
                .Property(c => c.CreatedOnUtc)
                .HasDefaultValueSql("NOW()");

            builder.Entity<Block>()
                .Property(b => b.CreatedOnUtc)
                .HasDefaultValueSql("NOW()");

            builder.Entity<Desire>()
               .Property(d => d.CreatedOnUtc)
               .HasDefaultValueSql("NOW()");

            builder.Entity<MasterCategory>()
                .Property(m => m.CreatedOnUtc)
                .HasDefaultValueSql("NOW()");

            builder.Entity<Professor>()
                .Property(p => p.CreatedOnUtc)
                .HasDefaultValueSql("NOW()");

            builder.Entity<Activity>()
                .Property(a => a.CreatedOnUtc)
                .HasDefaultValueSql("NOW()");

            builder.Entity<PersonalData>()
                .Property(p => p.CreatedOnUtc)
                .HasDefaultValueSql("NOW()");

            builder.Entity<ApplicationUser>()
                .Property(a => a.CreatedOnUtc)
                .HasDefaultValueSql("NOW()");
            builder.Entity<RefreshToken>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Entity<Album>()
              .Property(r => r.CreatedOnUtc)
              .HasDefaultValueSql("NOW()");

            builder.Entity<AlbumMedia>()
              .Property(r => r.CreatedOnUtc)
              .HasDefaultValueSql("NOW()");

        }

    }
    public class WriteDbContext : MainDbContext
    {
        public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options) { }
    }
    public class ReadDbContext : MainDbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }

        [Obsolete("This context is read-only", true)]
        public new int SaveChanges()
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        [Obsolete("This context is read-only", true)]
        public new int SaveChanges(bool acceptAll)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        [Obsolete("This context is read-only", true)]
        public new Task<int> SaveChangesAsync(CancellationToken token = default)
        {
            throw new InvalidOperationException("This context is read-only.");
        }

        [Obsolete("This context is read-only", true)]
        public new Task<int> SaveChangesAsync(bool acceptAll, CancellationToken token = default)
        {
            throw new InvalidOperationException("This context is read-only.");
        }
    }

}
