namespace Vasileuski.API.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; }

        // Навигационные свойства
        public List<TeamDTO>? Teams { get; set; }
    }

    public class CreateCategoryDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public class UpdateCategoryDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ExistingImage { get; set; }
    }
}
