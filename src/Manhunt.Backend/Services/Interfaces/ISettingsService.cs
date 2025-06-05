// Datei: Manhunt.Backend/Services/Interfaces/ISettingsService.cs
using Manhunt.Shared.DTOs;
using System.Threading.Tasks;

namespace Manhunt.Backend.Services.Interfaces
{
    public interface ISettingsService
    {
        Task<SettingsDto> CreateSettingsAsync(SettingsDto settings);
        Task<SettingsDto> GetSettingsByIdAsync(string settingsId);
        Task UpdateSettingsAsync(string settingsId, SettingsDto newSettings, string requesterId);
        Task DeleteSettingsAsync(string settingsId, string requesterId);
    }
}
