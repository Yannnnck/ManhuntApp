// Datei: Manhunt.Backend/Controllers/PlayerController.cs
using Manhunt.Backend.Services.Interfaces;
using Manhunt.Backend.Models.Requestsplayer;
using Manhunt.Shared.DTOs;
using Manhunt.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Manhunt.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        /// <summary>
        /// Spieler erstellt: Host oder automatischer Aufruf aus LobbyService
        /// Body: { "lobbyId": "...", "username": "...", "role": "Runner" }
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerCreatePlayerRequest req)
        {
            var dto = await _playerService.CreatePlayerAsync(req.LobbyId, req.Username, req.Role);
            return Ok(dto);
        }

        /// <summary>
        /// Holen alle Spieler in einer Lobby
        /// </summary>
        [HttpGet("lobby/{lobbyId}")]
        public async Task<IActionResult> GetPlayersByLobby(string lobbyId)
        {
            var players = await _playerService.GetPlayersByLobbyIdAsync(lobbyId);
            return Ok(players);
        }

        /// <summary>
        /// Spieler position aktualisieren (SignalR wäre eigentlich der direkte Weg, 
        /// aber als Backup: HTTP-Endpoint)
        /// Body: PositionDto
        /// </summary>
        [HttpPost("update-position")]
        public async Task<IActionResult> UpdatePosition([FromBody] PositionDto position)
        {
            await _playerService.UpdatePlayerPositionAsync(position);
            return NoContent();
        }

        /// <summary>
        /// Markiere Runner als gefangen
        /// Body: { "lobbyId": "...", "playerId": "...", "requesterId": "..." }
        /// </summary>
        [HttpPost("caught")]
        public async Task<IActionResult> MarkCaught([FromBody] PlayerMarkCaughtRequest req)
        {
            await _playerService.MarkPlayerCaughtAsync(req.LobbyId, req.PlayerId, req.RequesterId);
            return NoContent();
        }

        /// <summary>
        /// Spieler kicken (nur Host)
        /// Body: { "lobbyId": "...", "playerId": "...", "requesterId": "..." }
        /// </summary>
        [HttpDelete("{playerId}")]
        public async Task<IActionResult> DeletePlayer(string playerId, [FromBody] PlayerDeletePlayerRequest req)
        {
            await _playerService.DeletePlayerAsync(req.LobbyId, playerId, req.RequesterId);
            return NoContent();
        }

        /// <summary>
        /// Ready-Status setzen
        /// Body: { "playerId": "...", "isReady": true }
        /// </summary>
        [HttpPut("ready")]
        public async Task<IActionResult> ReadyStatus([FromBody] PlayerReadyRequest req)
        {
            await _playerService.MarkPlayerReadyAsync(req.PlayerId, req.IsReady);
            return NoContent();
        }

        #region DTOs für Requests

        #endregion
    }
}
