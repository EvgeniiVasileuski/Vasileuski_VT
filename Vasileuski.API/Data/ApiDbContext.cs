//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Reflection.Emit;
//using Vasileuski.Domain.Entities;

//namespace Vasileuski.API.Data
//{
//    public class ApiDbContext : DbContext
//    {
//        public ApiDbContext(DbContextOptions<ApiDbContext> options)
//            : base(options)
//        {
//        }

//        public DbSet<Team> Teams { get; set; }
//        public DbSet<Category> Categories { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // Настройка Team
//            modelBuilder.Entity<Team>(entity =>
//            {
//                entity.HasKey(e => e.Id);
//                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
//                entity.Property(e => e.Description).HasMaxLength(1000);
//                entity.Property(e => e.Location).HasMaxLength(100);
//                entity.Property(e => e.HeadCoach).HasMaxLength(100);
//                entity.Property(e => e.Captain).HasMaxLength(100);
//                entity.Property(e => e.Stadium).HasMaxLength(150);
//                entity.Property(e => e.Colors).HasMaxLength(200);
//                entity.Property(e => e.Image).HasMaxLength(500);

//                // Временные метки
//                entity.Property(e => e.CreatedAt).IsRequired();
//                entity.Property(e => e.UpdatedAt).IsRequired();

//                // Связь с Category
//                entity.HasOne(t => t.Category)
//                      .WithMany(c => c.Teams)
//                      .HasForeignKey(t => t.CategoryId)
//                      .OnDelete(DeleteBehavior.SetNull);
//            });

//            // Настройка Category
//            modelBuilder.Entity<Category>(entity =>
//            {
//                entity.HasKey(e => e.Id);
//                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
//                entity.Property(e => e.NormalizedName).IsRequired().HasMaxLength(100);
//                entity.Property(e => e.Description).HasMaxLength(500);
//                entity.Property(e => e.Image).HasMaxLength(500);

//                entity.HasIndex(e => e.NormalizedName).IsUnique();
//            });
//        }
//    }
//}