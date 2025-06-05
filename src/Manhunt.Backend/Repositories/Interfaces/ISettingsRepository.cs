// Datei: Manhunt.Backend/Repositories/Interfaces/ISettingsRepository.cs
using Manhunt.Backend.Models.Entities;
using System.Threading.Tasks;

namespace Manhunt.Backend.Repositories.Interfaces
{
    public interface ISettingsRepository
    {
        Task<SettingsEntity> CreateAsync(SettingsEntity settings);
        Task<SettingsEntity> GetByIdAsync(string settingsId);
        Task UpdateAsync(SettingsEntity settings);
        Task DeleteAsync(string settingsId);
    }
}
