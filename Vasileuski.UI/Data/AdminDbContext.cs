using Microsoft.EntityFrameworkCore;
using Vasileuski.Domain.Entities;

namespace Vasileuski.UI.Data
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка сущности Team
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Location).HasMaxLength(100);
                entity.Property(e => e.HeadCoach).HasMaxLength(100);
                entity.Property(e => e.Captain).HasMaxLength(100);
                entity.Property(e => e.Stadium).HasMaxLength(150);
                entity.Property(e => e.Colors).HasMaxLength(200);

                // Связь с Category
                entity.HasOne(t => t.Category)
                      .WithMany(c => c.Teams)
                      .HasForeignKey(t => t.CategoryId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Настройка сущности Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.NormalizedName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });
        }
    }
}