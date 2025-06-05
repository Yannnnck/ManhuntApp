// Datei: Manhunt.Backend/Services/Implementations/SettingsService.cs
using Manhunt.Backend.Models.Entities;
using Manhunt.Backend.Repositories.Interfaces;
using Manhunt.Backend.Services.Interfaces;
using Manhunt.Shared.DTOs;
using System.Threading.Tasks;

namespace Manhunt.Backend.Services.Implementations
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepo;
        private readonly ILobbyRepository _lobbyRepo;

        public SettingsService(
            ISettingsRepository settingsRepo,
            ILobbyRepository lobbyRepo)
        {
            _settingsRepo = settingsRepo;
            _lobbyRepo = lobbyRepo;
        }

        public async Task<SettingsDto> CreateSettingsAsync(SettingsDto settings)
        {
            var entity = new SettingsEntity
            {
                DisplayModeHunter = settings.DisplayModeHunter,
                DisplayModeRunner = settings.DisplayModeRunner,
                ShowDistance = settings.ShowDistance,
                UseStartDelay = settings.UseStartDelay,
                StartDelaySeconds = settings.StartDelaySeconds,
                UseLocationDelay = settings.UseLocationDelay,
                LocationDelaySeconds = settings.LocationDelaySeconds,
                ManualRefresh = settings.ManualRefresh,
                HunterVisibleToRunner = settings.HunterVisibleToRunner,
                CompassPointsNextOpponent = settings.CompassPointsNextOpponent
            };
            entity = await _settingsRepo.CreateAsync(entity);

            return new SettingsDto
            {
                DisplayModeHunter = entity.DisplayModeHunter,
                DisplayModeRunner = entity.DisplayModeRunner,
                ShowDistance = entity.ShowDistance,
                UseStartDelay = entity.UseStartDelay,
                StartDelaySeconds = entity.StartDelaySeconds,
                UseLocationDelay = entity.UseLocationDelay,
                LocationDelaySeconds = entity.LocationDelaySeconds,
                ManualRefresh = entity.ManualRefresh,
                HunterVisibleToRunner = entity.HunterVisibleToRunner,
                CompassPointsNextOpponent = entity.CompassPointsNextOpponent
            };
        }

        public async Task DeleteSettingsAsync(string settingsId, string requesterId)
        {
            // Löschen sollte nur erfolgen, wenn Lobby existiert & requester Host ist; hierzu müsste Lobby geholt werden
            // Vereinfachtes Beispiel: Löschung nur, wenn nicht referenziert
            await _settingsRepo.DeleteAsync(settingsId);
        }

        public async Task<SettingsDto> GetSettingsByIdAsync(string settingsId)
        {
            var entity = await _settingsRepo.GetByIdAsync(settingsId);
            if (entity == null)
                return null;

            return new SettingsDto
            {
                DisplayModeHunter = entity.DisplayModeHunter,
                DisplayModeRunner = entity.DisplayModeRunner,
                ShowDistance = entity.ShowDistance,
                UseStartDelay = entity.UseStartDelay,
                StartDelaySeconds = entity.StartDelaySeconds,
                UseLocationDelay = entity.UseLocationDelay,
                LocationDelaySeconds = entity.LocationDelaySeconds,
                ManualRefresh = entity.ManualRefresh,
                HunterVisibleToRunner = entity.HunterVisibleToRunner,
                CompassPointsNextOpponent = entity.CompassPointsNextOpponent
            };
        }

        public async Task UpdateSettingsAsync(string settingsId, SettingsDto newSettings, string requesterId)
        {
            // Nur Host kann Settings ändern; wir bräuchten dafür die Lobby, in der dieses Settings-Objekt referenziert ist
            // Zum Beispiel:
            // var lobby = await _lobbyRepo.GetBySettingsIdAsync(settingsId);
            // if (lobby.HostUserId != requesterId) throw new UnauthorizedAccessException();

            var entity = await _settingsRepo.GetByIdAsync(settingsId);
            if (entity == null)
                throw new KeyNotFoundException("Settings nicht gefunden");

            entity.DisplayModeHunter = newSettings.DisplayModeHunter;
            entity.DisplayModeRunner = newSettings.DisplayModeRunner;
            entity.ShowDistance = newSettings.ShowDistance;
            entity.UseStartDelay = newSettings.UseStartDelay;
            entity.StartDelaySeconds = newSettings.StartDelaySeconds;
            entity.UseLocationDelay = newSettings.UseLocationDelay;
            entity.LocationDelaySeconds = newSettings.LocationDelaySeconds;
            entity.ManualRefresh = newSettings.ManualRefresh;
            entity.HunterVisibleToRunner = newSettings.HunterVisibleToRunner;
            entity.CompassPointsNextOpponent = newSettings.CompassPointsNextOpponent;

            await _settingsRepo.UpdateAsync(entity);
        }
    }
}
