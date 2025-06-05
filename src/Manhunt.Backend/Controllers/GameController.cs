// Datei: Manhunt.Backend/Controllers/GameController.cs
using Manhunt.Backend.Services.Interfaces;
using Manhunt.Backend.Models.Requestsgame;
using Manhunt.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manhunt.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Host startet das Spiel und wählt die Runner aus
        /// Body: { "lobbyId": "...", "runnerIds": ["id1","id2"], "requesterId": "..." }
        /// </summary>
        [HttpPost("start")]
        public async Task<IActionResult> StartGame([FromBody] GameStartGameRequest req)
        {
            await _gameService.StartGameAsync(req.LobbyId, req.RequesterId, req.RunnerIds);
            return NoContent();
        }

        /// <summary>
        /// Host beendet das Spiel
        /// Body: { "lobbyId": "...", "requesterId": "..." }
        /// </summary>
        [HttpPost("end")]
        public async Task<IActionResult> EndGame([FromBody] GameEndGameRequest req)
        {
            await _gameService.EndGameAsync(req.LobbyId, req.RequesterId);
            return NoContent();
        }

        /// <summary>
        /// Host pausiert oder setzt das Spiel fort
        /// Body: { "lobbyId": "...", "requesterId": "..." }
        /// </summary>
        [HttpPost("pause")]
        public async Task<IActionResult> PauseGame([FromBody] GamePauseGameRequest req)
        {
            await _gameService.PauseGameAsync(req.LobbyId, req.RequesterId);
            return NoContent();
        }

        /// <summary>
        /// Gibt alle aktuellen Positionen aller Spieler in einer Lobby zurück
        /// </summary>
        [HttpGet("positions/{lobbyId}")]
        public async Task<IActionResult> GetPositions(string lobbyId)
        {
            var positions = await _gameService.GetAllPositionsAsync(lobbyId);
            return Ok(positions);
        }

        #region DTOs für Requests

        #endregion
    }
}
