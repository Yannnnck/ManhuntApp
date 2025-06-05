// Datei: Manhunt.Backend/Repositories/Implementations/PlayerRepository.cs
using Manhunt.Backend.Models.Entities;
using Manhunt.Backend.Repositories.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manhunt.Backend.Repositories.Implementations
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IMongoCollection<PlayerEntity> _players;

        public PlayerRepository(IMongoDatabase database)
        {
            _players = database.GetCollection<PlayerEntity>("Players");
        }

        public async Task<PlayerEntity> CreateAsync(PlayerEntity player)
        {
            await _players.InsertOneAsync(player);
            return player;
        }

        public async Task DeleteAsync(string playerId)
        {
            var filter = Builders<PlayerEntity>.Filter.Eq(p => p.PlayerId, playerId);
            await _players.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<PlayerEntity>> GetByLobbyIdAsync(string lobbyId)
        {
            var filter = Builders<PlayerEntity>.Filter.Eq(p => p.LobbyId, lobbyId);
            return await _players.Find(filter).ToListAsync();
        }

        public async Task<PlayerEntity> GetByIdAsync(string playerId)
        {
            var filter = Builders<PlayerEntity>.Filter.Eq(p => p.PlayerId, playerId);
            return await _players.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(PlayerEntity player)
        {
            var filter = Builders<PlayerEntity>.Filter.Eq(p => p.PlayerId, player.PlayerId);
            await _players.ReplaceOneAsync(filter, player);
        }
    }
}
