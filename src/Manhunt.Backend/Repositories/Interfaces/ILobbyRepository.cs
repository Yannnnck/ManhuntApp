// Datei: Manhunt.Backend/Repositories/Interfaces/ILobbyRepository.cs
using Manhunt.Backend.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manhunt.Backend.Repositories.Interfaces
{
    public interface ILobbyRepository
    {
        Task<LobbyEntity> CreateAsync(LobbyEntity lobby);
        Task<LobbyEntity> GetByIdAsync(string lobbyId);
        Task<LobbyEntity> GetByCodeAsync(string code);
        Task<IEnumerable<LobbyEntity>> GetAllAsync();
        Task UpdateAsync(LobbyEntity lobby);
        Task DeleteAsync(string lobbyId);
    }
}
