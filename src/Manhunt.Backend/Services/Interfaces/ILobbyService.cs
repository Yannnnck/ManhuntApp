// Datei: Manhunt.Backend/Services/Interfaces/ILobbyService.cs
using Manhunt.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manhunt.Backend.Services.Interfaces
{
    public interface ILobbyService
    {
        Task<LobbyDto> CreateLobbyAsync(string hostUserId, string hostUsername, SettingsDto initialSettings);
        Task<LobbyDto> GetLobbyByIdAsync(string lobbyId);
        Task<LobbyDto> GetLobbyByCodeAsync(string code);
        Task<IEnumerable<LobbyDto>> GetAllLobbiesAsync();
        Task JoinLobbyAsync(string lobbyId, string username, string userId);
        Task LeaveLobbyAsync(string lobbyId, string playerId);
        Task UpdateLobbySettingsAsync(string lobbyId, SettingsDto newSettings, string requesterId);
        // optional: KickPlayer, SelectRunners, StartGame, EndGame, etc. 
    }
}
