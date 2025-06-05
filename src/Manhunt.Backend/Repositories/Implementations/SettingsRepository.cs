// Datei: Manhunt.Backend/Repositories/Implementations/SettingsRepository.cs
using Manhunt.Backend.Models.Entities;
using Manhunt.Backend.Repositories.Interfaces;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Manhunt.Backend.Repositories.Implementations
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly IMongoCollection<SettingsEntity> _settingsCollection;

        public SettingsRepository(IMongoDatabase database)
        {
            _settingsCollection = database.GetCollection<SettingsEntity>("Settings");
        }

        public async Task<SettingsEntity> CreateAsync(SettingsEntity settings)
        {
            await _settingsCollection.InsertOneAsync(settings);
            return settings;
        }

        public async Task DeleteAsync(string settingsId)
        {
            var filter = Builders<SettingsEntity>.Filter.Eq(s => s.SettingsId, settingsId);
            await _settingsCollection.DeleteOneAsync(filter);
        }

        public async Task<SettingsEntity> GetByIdAsync(string settingsId)
        {
            var filter = Builders<SettingsEntity>.Filter.Eq(s => s.SettingsId, settingsId);
            return await _settingsCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(SettingsEntity settings)
        {
            var filter = Builders<SettingsEntity>.Filter.Eq(s => s.SettingsId, settings.SettingsId);
            await _settingsCollection.ReplaceOneAsync(filter, settings);
        }
    }
}
