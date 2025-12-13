using Microsoft.EntityFrameworkCore;
using Vasileuski.Domain.Entities;

namespace Vasileuski.UI.Data
{
    public class TempDbContext : DbContext
    {
        public TempDbContext(DbContextOptions<TempDbContext> options) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка Team
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

                // Связь с Category (если нужно)
                entity.HasOne(e => e.Category)
                      .WithMany()
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Настройка Category
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