using Vasileuski.Domain.Entities;

namespace Vasileuski.UI.Models
{
    public class StatisticsViewModel
    {
        public int TotalTeams { get; set; }
        public int TotalPoints { get; set; }
        public double AveragePoints { get; set; }
        public int MaxPoints { get; set; }
        public int MinPoints { get; set; }
        public Team? TopTeam { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Team> Teams { get; set; } = new List<Team>();
    }
}
