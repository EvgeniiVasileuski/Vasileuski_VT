using Microsoft.AspNetCore.Identity;

namespace Vasileuski.UI.Data
{
    /// <summary>
    /// Расширенный класс пользователя для системы Identity
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Аватар пользователя в формате byte[]
        /// </summary>
        public byte[]? Avatar { get; set; }

        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Дата последнего входа
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Биография/описание пользователя
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// Страна пользователя
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Город пользователя
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Пол: true - мужской, false - женский, null - не указан
        /// </summary>
        public bool? Gender { get; set; }

        /// <summary>
        /// URL веб-сайта пользователя
        /// </summary>
        public string? Website { get; set; }

        /// <summary>
        /// Номер телефона (дополнительный)
        /// </summary>
        public string? PhoneNumber2 { get; set; }

        /// <summary>
        /// Признак онлайн-статуса
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Количество очков/рейтинг пользователя
        /// </summary>
        public int Points { get; set; } = 0;

        /// <summary>
        /// Уровень пользователя
        /// </summary>
        public int Level { get; set; } = 1;

        /// <summary>
        /// Настройки уведомлений пользователя (можно хранить как JSON)
        /// </summary>
        public string? NotificationSettings { get; set; }

        /// <summary>
        /// Язык интерфейса
        /// </summary>
        public string? PreferredLanguage { get; set; } = "ru-RU";

        /// <summary>
        /// Часовой пояс
        /// </summary>
        public string? TimeZone { get; set; } = "UTC";

        /// <summary>
        /// Тема оформления
        /// </summary>
        public string? Theme { get; set; } = "light";

        /// <summary>
        /// Дополнительные поля для профиля
        /// </summary>
        public string? Position { get; set; } // Должность
        public string? Company { get; set; }  // Компания
        public string? Department { get; set; } // Отдел

        /// <summary>
        /// Социальные сети (можно хранить как JSON)
        /// </summary>
        public string? SocialNetworks { get; set; }

        /// <summary>
        /// Верификация аккаунта
        /// </summary>
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Дата последнего обновления профиля
        /// </summary>
        public DateTime? ProfileUpdatedAt { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ApplicationUser() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public ApplicationUser(string userName) : base(userName)
        {
            Email = userName;
        }

        /// <summary>
        /// Метод для расчета возраста (если указана дата рождения)
        /// </summary>
        public int? GetAge()
        {
            if (!DateOfBirth.HasValue)
                return null;

            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Value.Year;

            // Проверяем, прошел ли уже день рождения в текущем году
            if (DateOfBirth.Value.Date > today.AddYears(-age))
                age--;

            return age;
        }

        /// <summary>
        /// Метод для получения форматированного имени
        /// </summary>
        public string GetDisplayName()
        {
            if (!string.IsNullOrEmpty(FullName))
                return FullName;

            if (!string.IsNullOrEmpty(UserName))
                return UserName;

            return Email?.Split('@')[0] ?? "Пользователь";
        }

        /// <summary>
        /// Метод для получения инициалов
        /// </summary>
        public string GetInitials()
        {
            if (!string.IsNullOrEmpty(FullName))
            {
                var parts = FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    return $"{parts[0][0]}{parts[1][0]}".ToUpper();
                }
                else if (parts.Length == 1)
                {
                    return parts[0][0].ToString().ToUpper();
                }
            }

            if (!string.IsNullOrEmpty(UserName))
            {
                return UserName[0].ToString().ToUpper();
            }

            return "U"; // User
        }

        /// <summary>
        /// Метод для увеличения очков пользователя
        /// </summary>
        public void AddPoints(int points)
        {
            if (points > 0)
            {
                Points += points;

                // Автоматическое повышение уровня (каждые 100 очков)
                Level = (Points / 100) + 1;
            }
        }
    }
}