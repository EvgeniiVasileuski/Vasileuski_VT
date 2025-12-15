namespace Vasileuski.API.DTOs
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Points { get; set; }
        public int? CategoryId { get; set; }
        public string? Image { get; set; }
        public string? Location { get; set; }
        public int? FoundedYear { get; set; }
        public string? HeadCoach { get; set; }
        public string? Captain { get; set; }
        public string? Stadium { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Position { get; set; }
        public string? Colors { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Навигационные свойства
        public CategoryDTO? Category { get; set; }

        // Вычисляемые свойства
        public int GoalDifference => GoalsFor - GoalsAgainst;
        public int TotalGames => Wins + Draws + Losses;
        public double WinPercentage => TotalGames > 0 ? (double)Wins / TotalGames * 100 : 0;
    }

    public class CreateTeamDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Points { get; set; } = 0;
        public int? CategoryId { get; set; }
        public string? Location { get; set; }
        public int? FoundedYear { get; set; }
        public string? HeadCoach { get; set; }
        public string? Captain { get; set; }
        public string? Stadium { get; set; }
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Draws { get; set; } = 0;
        public int GoalsFor { get; set; } = 0;
        public int GoalsAgainst { get; set; } = 0;
        public int Position { get; set; } = 0;
        public string? Colors { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public class UpdateTeamDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Points { get; set; }
        public int? CategoryId { get; set; }
        public string? Location { get; set; }
        public int? FoundedYear { get; set; }
        public string? HeadCoach { get; set; }
        public string? Captain { get; set; }
        public string? Stadium { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Position { get; set; }
        public string? Colors { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ExistingImage { get; set; }
    }
}
