// Datei: Manhunt.Backend/Controllers/VoteController.cs
using Manhunt.Backend.Services.Interfaces;
using Manhunt.Backend.Models.Requestsvote;
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
    public class VoteController : ControllerBase
    {
        private readonly IGameService _gameService; // wir verwenden GameService, um Votes zu verarbeiten

        public VoteController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Host startet Abstimmung (Restart oder EndGame)
        /// Body: { "lobbyId": "...", "requestType": "Restart", "requesterId": "..." }
        /// </summary>
        [HttpPost("start")]
        public async Task<IActionResult> StartVote([FromBody] VoteStartVoteRequest req)
        {
            // In GameService könnte man z.B. voten initialisieren
            // await _voteService.StartVoteAsync(req.LobbyId, req.RequestType, req.RequesterId);
            return NoContent();
        }

        /// <summary>
        /// Spieler gibt Stimme ab
        /// Body: VoteDto (enthält LobbyId, PlayerId, VoteFor, RequestType)
        /// </summary>
        [HttpPost("cast")]
        public async Task<IActionResult> CastVote([FromBody] VoteDto voteDto)
        {
            // await _voteService.CastVoteAsync(voteDto);
            return NoContent();
        }

        /// <summary>
        /// Gibt aktuellen Abstimmungsstand zurück (Ja-/Nein-Prozente)
        /// </summary>
        [HttpGet("{lobbyId}")]
        public async Task<IActionResult> GetVoteResult(string lobbyId)
        {
            // var result = await _voteService.GetVoteResultAsync(lobbyId);
            // return Ok(result);
            return Ok(new { VotesFor = 0, VotesAgainst = 0, PercentageFor = 0.0 });
        }

        #region DTOs für Requests

        #endregion
    }
}
