using Vasileuski.Domain.Entities;
using Vasileuski.Domain.Models;

namespace Vasileuski.UI.Services
{
    /// <summary>
    /// Сервис для работы с категориями в памяти
    /// </summary>
    public class MemoryCategoryService : ICategoryService
    {
        private List<Category> _categories;

        public MemoryCategoryService()
        {
            SetupData();
        }

        /// <summary>
        /// Наполнение данных категориями
        /// </summary>
        private void SetupData()
        {
            _categories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Футбол",
                    NormalizedName = "football",
                    Description = "Футбольные лиги и турниры",
                    Image = "/images/categories/football.png"
                },
                new Category
                {
                    Id = 2,
                    Name = "Баскетбол",
                    NormalizedName = "basketball",
                    Description = "Баскетбольные лиги и турниры",
                    Image = "/images/categories/basketball.png"
                },
                new Category
                {
                    Id = 3,
                    Name = "Хоккей",
                    NormalizedName = "hockey",
                    Description = "Хоккейные лиги и турниры",
                    Image = "/images/categories/hockey.png"
                },
                new Category
                {
                    Id = 4,
                    Name = "Теннис",
                    NormalizedName = "tennis",
                    Description = "Теннисные турниры и ассоциации",
                    Image = "/images/categories/tennis.png"
                },
                new Category
                {
                    Id = 5,
                    Name = "Волейбол",
                    NormalizedName = "volleyball",
                    Description = "Волейбольные лиги и турниры",
                    Image = "/images/categories/volleyball.png"
                }
            };
        }

        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var result = new ResponseData<List<Category>>
            {
                Data = _categories,
                Success = true
            };

            return Task.FromResult(result);
        }
    }
}