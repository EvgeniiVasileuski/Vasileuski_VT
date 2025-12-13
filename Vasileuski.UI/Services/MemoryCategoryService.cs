using Vasileuski.Domain.Entities;

namespace Vasileuski.UI.Services
{
    public class MemoryCategoryService : ICategoryService
    {
        private readonly List<Category> _categories = new List<Category>();
        private int _nextId = 1;

        public MemoryCategoryService()
        {
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            _categories.Add(new Category
            {
                Id = _nextId++,
                Name = "Хоккей",
                NormalizedName = "hockey",
                Description = "Хоккейные команды",
                Image = "/images/categories/hockey.png" // Добавьте свойство Image в модель Category
            });

            _categories.Add(new Category
            {
                Id = _nextId++,
                Name = "Футбол",
                NormalizedName = "football",
                Description = "Футбольные команды",
                Image = "/images/categories/football.png"
            });

            _categories.Add(new Category
            {
                Id = _nextId++,
                Name = "Баскетбол",
                NormalizedName = "basketball",
                Description = "Баскетбольные команды",
                Image = "/images/categories/basketball.png"
            });

            _categories.Add(new Category
            {
                Id = _nextId++,
                Name = "Теннис",
                NormalizedName = "tennis",
                Description = "Теннисные команды",
                Image = "/images/categories/tennis.png"
            });

            _categories.Add(new Category
            {
                Id = _nextId++,
                Name = "Волейбол",
                NormalizedName = "volleyball",
                Description = "Волейбольные команды",
                Image = "/images/categories/volleyball.png"
            });
        }

        // Исправленный метод - возвращает Task<ServiceResponse<IEnumerable<Category>>>
        public Task<ServiceResponse<IEnumerable<Category>>> GetCategoryListAsync()
        {
            try
            {
                return Task.FromResult(ServiceResponse<IEnumerable<Category>>.Ok(_categories));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ServiceResponse<IEnumerable<Category>>.Error(
                    $"Ошибка при получении категорий: {ex.Message}"));
            }
        }

        public Task<ServiceResponse<Category>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = _categories.FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    return Task.FromResult(ServiceResponse<Category>.Error(
                        $"Категория с ID {id} не найдена"));
                }

                return Task.FromResult(ServiceResponse<Category>.Ok(category));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ServiceResponse<Category>.Error(
                    $"Ошибка при получении категории: {ex.Message}"));
            }
        }
    }
}