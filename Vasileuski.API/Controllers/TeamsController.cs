using Microsoft.AspNetCore.Mvc;
using Vasileuski.API.DTOs;
using Vasileuski.API.Services;

namespace Vasileuski.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly IWebHostEnvironment _environment;

        public TeamsController(ITeamService teamService, IWebHostEnvironment environment)
        {
            _teamService = teamService;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeams([FromQuery] string? category = null)
        {
            var result = await _teamService.GetTeamsAsync(category);
            if (!result.Success)
                return BadRequest(new { error = result.ErrorMessage });

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam(int id)
        {
            var result = await _teamService.GetTeamByIdAsync(id);
            if (!result.Success)
                return NotFound(new { error = result.ErrorMessage });

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromForm] CreateTeamDTO teamDto)
        {
            var result = await _teamService.CreateTeamAsync(teamDto);
            if (!result.Success)
                return BadRequest(new { error = result.ErrorMessage });

            return CreatedAtAction(nameof(GetTeam), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromForm] UpdateTeamDTO teamDto)
        {
            var result = await _teamService.UpdateTeamAsync(id, teamDto);
            if (!result.Success)
                return BadRequest(new { error = result.ErrorMessage });

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var result = await _teamService.DeleteTeamAsync(id);
            if (!result.Success)
                return BadRequest(new { error = result.ErrorMessage });

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTeams([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest(new { error = "Search term is required" });

            var result = await _teamService.SearchTeamsAsync(term);
            if (!result.Success)
                return BadRequest(new { error = result.ErrorMessage });

            return Ok(result.Data);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await _teamService.GetStatisticsAsync();
            if (!result.Success)
                return BadRequest(new { error = result.ErrorMessage });

            return Ok(result.Data);
        }

        [HttpGet("top/{count}")]
        public async Task<IActionResult> GetTopTeams(int count)
        {
            var result = await _teamService.GetTeamsAsync(null);
            if (!result.Success)
                return BadRequest(new { error = result.ErrorMessage });

            var topTeams = result.Data
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference)
                .Take(count)
                .ToList();

            return Ok(topTeams);
        }
    }
}