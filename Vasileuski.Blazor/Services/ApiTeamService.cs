using System.Net.Http.Json;
using Vasileuski.Blazor.Models;

namespace Vasileuski.Blazor.Services
{
    public class ApiTeamService : ITeamService
    {
        private readonly HttpClient _http;
        private List<TeamDTO> _teams = new();
        private int _currentPage = 1;
        private int _totalPages = 1;

        public ApiTeamService(HttpClient http)
        {
            _http = http;
        }

        public IEnumerable<TeamDTO> Teams => _teams;
        public int CurrentPage => _currentPage;
        public int TotalPages => _totalPages;

        public event Action? ListChanged;

        public async Task GetTeams(int pageNo = 1, int pageSize = 3)
        {
            try
            {
                var result = await _http.GetAsync("teams");

                if (result.IsSuccessStatusCode)
                {
                    var responseData = await result.Content
                        .ReadFromJsonAsync<ResponseData<List<TeamDTO>>>();

                    if (responseData?.Data != null)
                    {
                        _currentPage = pageNo;
                        _totalPages = (int)Math.Ceiling(responseData.Data.Count / (double)pageSize);

                        _teams = responseData.Data
                            .Skip((pageNo - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

                        ListChanged?.Invoke();
                    }
                }
                else
                {
                    _teams = new List<TeamDTO>();
                    _currentPage = 1;
                    _totalPages = 0;
                    ListChanged?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении команд: {ex.Message}");
                _teams = new List<TeamDTO>();
                _currentPage = 1;
                _totalPages = 0;
                ListChanged?.Invoke();
            }
        }
    }
}
