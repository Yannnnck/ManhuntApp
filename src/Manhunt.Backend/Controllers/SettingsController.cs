// Datei: Manhunt.Backend/Controllers/SettingsController.cs
using Manhunt.Backend.Services.Interfaces;
using Manhunt.Backend.Models.Requestssettings;
using Manhunt.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Manhunt.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet("{settingsId}")]
        public async Task<IActionResult> GetSettings(string settingsId)
        {
            var settings = await _settingsService.GetSettingsByIdAsync(settingsId);
            if (settings == null) return NotFound();
            return Ok(settings);
        }

        [HttpPut("{settingsId}")]
        public async Task<IActionResult> UpdateSettings(string settingsId, [FromBody] SettingsUpdateSettingsRequest req)
        {
            await _settingsService.UpdateSettingsAsync(settingsId, req.NewSettings, req.RequesterId);
            return NoContent();
        }

        #region Hilfsklassen

        #endregion
    }
}
