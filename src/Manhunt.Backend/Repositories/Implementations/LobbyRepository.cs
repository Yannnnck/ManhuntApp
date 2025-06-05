// Datei: Manhunt.Backend/Repositories/Implementations/LobbyRepository.cs
using Manhunt.Backend.Models.Entities;
using Manhunt.Backend.Repositories.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manhunt.Backend.Repositories.Implementations
{
    public class LobbyRepository : ILobbyRepository
    {
        private readonly IMongoCollection<LobbyEntity> _lobbies;

        public LobbyRepository(IMongoDatabase database)
        {
            _lobbies = database.GetCollection<LobbyEntity>("Lobbies");
        }

        public async Task<LobbyEntity> CreateAsync(LobbyEntity lobby)
        {
            await _lobbies.InsertOneAsync(lobby);
            return lobby;
        }

        public async Task DeleteAsync(string lobbyId)
        {
            var filter = Builders<LobbyEntity>.Filter.Eq(l => l.LobbyId, lobbyId);
            await _lobbies.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<LobbyEntity>> GetAllAsync()
        {
            return await _lobbies.Find(_ => true).ToListAsync();
        }

        public async Task<LobbyEntity> GetByCodeAsync(string code)
        {
            var filter = Builders<LobbyEntity>.Filter.Eq(l => l.Code, code);
            return await _lobbies.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<LobbyEntity> GetByIdAsync(string lobbyId)
        {
            var filter = Builders<LobbyEntity>.Filter.Eq(l => l.LobbyId, lobbyId);
            return await _lobbies.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(LobbyEntity lobby)
        {
            var filter = Builders<LobbyEntity>.Filter.Eq(l => l.LobbyId, lobby.LobbyId);
            await _lobbies.ReplaceOneAsync(filter, lobby);
        }
    }
}
