using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vasileuski.UI.Data;  // Добавьте этот using

namespace Vasileuski.UI.Data
{
    // ИЗМЕНЕНИЕ: Укажите ApplicationUser как тип пользователя
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Настройка таблицы Users (ApplicationUser)
            builder.Entity<ApplicationUser>(entity =>
            {
                // Настройка свойств
                entity.Property(e => e.FullName)
                    .HasMaxLength(100);

                entity.Property(e => e.Bio)
                    .HasMaxLength(1000);

                entity.Property(e => e.Country)
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .HasMaxLength(50);

                entity.Property(e => e.Website)
                    .HasMaxLength(200);

                entity.Property(e => e.PhoneNumber2)
                    .HasMaxLength(20);

                entity.Property(e => e.PreferredLanguage)
                    .HasMaxLength(10)
                    .HasDefaultValue("ru-RU");

                entity.Property(e => e.TimeZone)
                    .HasMaxLength(50)
                    .HasDefaultValue("UTC");

                entity.Property(e => e.Theme)
                    .HasMaxLength(10)
                    .HasDefaultValue("light");

                entity.Property(e => e.Position)
                    .HasMaxLength(100);

                entity.Property(e => e.Company)
                    .HasMaxLength(100);

                entity.Property(e => e.Department)
                    .HasMaxLength(100);

                // Индексы
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasIndex(e => e.UserName)
                    .IsUnique();

                entity.HasIndex(e => e.FullName);

                entity.HasIndex(e => e.RegistrationDate);

                entity.HasIndex(e => e.IsOnline);

                entity.HasIndex(e => e.Points);

                // Настройка аватара (храним в базе как varbinary(max))
                entity.Property(e => e.Avatar)
                    .HasColumnType("varbinary(max)")
                    .IsRequired(false);
            });

            // Можно добавить настройки для других таблиц Identity, если нужно
        }
    }
}