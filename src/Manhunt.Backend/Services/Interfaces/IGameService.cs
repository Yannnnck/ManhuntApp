// Datei: Manhunt.Backend/Services/Interfaces/IGameService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Manhunt.Shared.DTOs;
using Manhunt.Shared.Enums;

namespace Manhunt.Backend.Services.Interfaces
{
    public interface IGameService
    {
        Task StartGameAsync(string lobbyId, string requesterId, IEnumerable<string> runnerIds);
        Task EndGameAsync(string lobbyId, string requesterId);
        Task PauseGameAsync(string lobbyId, string requesterId);
        Task SelectRunnersAsync(string lobbyId, IEnumerable<string> runnerIds, string requesterId);
        Task<IEnumerable<PositionDto>> GetAllPositionsAsync(string lobbyId);
        // Weitere Methoden für Voting, Status-Abfragen etc.
    }
}
