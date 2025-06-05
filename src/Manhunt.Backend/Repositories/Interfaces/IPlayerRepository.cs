// Datei: Manhunt.Backend/Repositories/Interfaces/IPlayerRepository.cs
using Manhunt.Backend.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manhunt.Backend.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        Task<PlayerEntity> CreateAsync(PlayerEntity player);
        Task<PlayerEntity> GetByIdAsync(string playerId);
        Task<IEnumerable<PlayerEntity>> GetByLobbyIdAsync(string lobbyId);
        Task UpdateAsync(PlayerEntity player);
        Task DeleteAsync(string playerId);
    }
}
