// Datei: Manhunt.Backend/Controllers/LobbyController.cs
using Manhunt.Backend.Services.Interfaces;
using Manhunt.Backend.Models.Requestslobby;
using Manhunt.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Manhunt.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Nur mit JWT darf man hier rein
    public class LobbyController : ControllerBase
    {
        private readonly ILobbyService _lobbyService;

        public LobbyController(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        /// <summary>
        /// Host erstellt eine neue Lobby und wird automatisch als Hunter angelegt
        /// Body: { "hostUserId": "...", "hostUsername": "...", "initialSettings": { ... } }
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateLobby([FromBody] LobbyCreateLobbyRequest req)
        {
            var dto = await _lobbyService.CreateLobbyAsync(req.HostUserId, req.HostUsername, req.InitialSettings);
            return Ok(dto);
        }

        /// <summary>
        /// Gibt Lobbys nach ID zurück
        /// </summary>
        [HttpGet("{lobbyId}")]
        public async Task<IActionResult> GetLobby(string lobbyId)
        {
            var lobby = await _lobbyService.GetLobbyByIdAsync(lobbyId);
            return Ok(lobby);
        }

        /// <summary>
        /// Host oder Spieler holt alle Lobbys
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lobbies = await _lobbyService.GetAllLobbiesAsync();
            return Ok(lobbies);
        }

        /// <summary>
        /// Spieler tritt einer Lobby bei
        /// Body: { "lobbyId": "...", "username": "...", "userId": "..." }
        /// </summary>
        [HttpPost("join")]
        public async Task<IActionResult> JoinLobby([FromBody] LobbyJoinLobbyRequest req)
        {
            await _lobbyService.JoinLobbyAsync(req.LobbyId, req.Username, req.UserId);
            return NoContent();
        }

        /// <summary>
        /// Spieler verlässt Lobby
        /// Body: { "lobbyId": "...", "playerId": "..." }
        /// </summary>
        [HttpPost("leave")]
        public async Task<IActionResult> LeaveLobby([FromBody] LobbyLeaveLobbyRequest req)
        {
            await _lobbyService.LeaveLobbyAsync(req.LobbyId, req.PlayerId);
            return NoContent();
        }

        /// <summary>
        /// Host aktualisiert die Settings 
        /// Body: { "lobbyId": "...", "newSettings": {...}, "requesterId": "..." }
        /// </summary>
        [HttpPut("settings")]
        public async Task<IActionResult> UpdateSettings([FromBody] LobbyUpdateSettingsRequest req)
        {
            await _lobbyService.UpdateLobbySettingsAsync(req.LobbyId, req.NewSettings, req.RequesterId);
            return NoContent();
        }

        #region Hilfsklassen für Requests

        #endregion
    }
}
