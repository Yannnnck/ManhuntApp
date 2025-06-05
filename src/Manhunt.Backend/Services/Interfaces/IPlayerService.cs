// Datei: Manhunt.Backend/Services/Interfaces/IPlayerService.cs
using Manhunt.Shared.DTOs;
using Manhunt.Shared.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manhunt.Backend.Services.Interfaces
{
    public interface IPlayerService
    {
        Task<PlayerDto> CreatePlayerAsync(string lobbyId, string username, PlayerRole role);
        Task<PlayerDto> GetPlayerByIdAsync(string playerId);
        Task<IEnumerable<PlayerDto>> GetPlayersByLobbyIdAsync(string lobbyId);
        Task UpdatePlayerPositionAsync(PositionDto position);
        Task MarkPlayerReadyAsync(string playerId, bool isReady);
        Task MarkPlayerCaughtAsync(string lobbyId, string playerId, string requesterId);
        Task DeletePlayerAsync(string lobbyId, string playerId, string requesterId);
    }
}
