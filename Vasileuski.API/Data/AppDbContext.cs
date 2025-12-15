using Microsoft.EntityFrameworkCore;
using Vasileuski.Domain.Entities;

namespace Vasileuski.API.Data
{
    /// <summary>
    /// Контекст базы данных для API проекта
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSet для сущностей предметной области
        public DbSet<Team> Teams { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация сущности Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.NormalizedName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Description)
                    .HasMaxLength(500);
                entity.Property(e => e.Image)
                    .HasMaxLength(500);

                // Уникальный индекс для NormalizedName
                entity.HasIndex(e => e.NormalizedName)
                    .IsUnique();

                // Связь с Teams
                entity.HasMany(c => c.Teams)
                    .WithOne(t => t.Category)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Конфигурация сущности Team
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Description)
                    .HasMaxLength(1000);
                entity.Property(e => e.Location)
                    .HasMaxLength(100);
                entity.Property(e => e.HeadCoach)
                    .HasMaxLength(100);
                entity.Property(e => e.Captain)
                    .HasMaxLength(100);
                entity.Property(e => e.Stadium)
                    .HasMaxLength(150);
                entity.Property(e => e.Colors)
                    .HasMaxLength(200);
                entity.Property(e => e.Image)
                    .HasMaxLength(500);
                entity.Property(e => e.CreatedAt)
                    .IsRequired();
                entity.Property(e => e.UpdatedAt)
                    .IsRequired();

                // Связь с Category
                entity.HasOne(t => t.Category)
                    .WithMany(c => c.Teams)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}