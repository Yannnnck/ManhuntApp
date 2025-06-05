// Datei: Manhunt.Backend/Services/Implementations/LobbyService.cs
using Manhunt.Backend.Models.Entities;
using Manhunt.Backend.Repositories.Interfaces;
using Manhunt.Backend.Services.Interfaces;
using Manhunt.Shared.DTOs;
using Manhunt.Shared.Enums;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manhunt.Backend.Services.Implementations
{
    public class LobbyService : ILobbyService
    {
        private readonly ILobbyRepository _lobbyRepo;
        private readonly ISettingsRepository _settingsRepo;
        private readonly IPlayerRepository _playerRepo;

        public LobbyService(
            ILobbyRepository lobbyRepo,
            ISettingsRepository settingsRepo,
            IPlayerRepository playerRepo)
        {
            _lobbyRepo = lobbyRepo;
            _settingsRepo = settingsRepo;
            _playerRepo = playerRepo;
        }

        public async Task<LobbyDto> CreateLobbyAsync(string hostUserId, string hostUsername, SettingsDto initialSettings)
        {
            // 1. Settings in DB anlegen
            var settingsEntity = new SettingsEntity
            {
                DisplayModeHunter = initialSettings.DisplayModeHunter,
                DisplayModeRunner = initialSettings.DisplayModeRunner,
                ShowDistance = initialSettings.ShowDistance,
                UseStartDelay = initialSettings.UseStartDelay,
                StartDelaySeconds = initialSettings.StartDelaySeconds,
                UseLocationDelay = initialSettings.UseLocationDelay,
                LocationDelaySeconds = initialSettings.LocationDelaySeconds,
                ManualRefresh = initialSettings.ManualRefresh,
                HunterVisibleToRunner = initialSettings.HunterVisibleToRunner,
                CompassPointsNextOpponent = initialSettings.CompassPointsNextOpponent
            };
            settingsEntity = await _settingsRepo.CreateAsync(settingsEntity);

            // 2. Lobby in DB anlegen (mit zufälligem Code)
            var random = new Random();
            string code;
            do
            {
                code = random.Next(1000, 9999).ToString(); // 4-stelliger Code als Beispiel
            } while (await _lobbyRepo.GetByCodeAsync(code) != null);

            var lobbyEntity = new LobbyEntity
            {
                Code = code,
                HostUserId = hostUserId,
                SettingsId = settingsEntity.SettingsId,
                Status = LobbyStatus.Created
            };
            lobbyEntity = await _lobbyRepo.CreateAsync(lobbyEntity);

            // 3. Host als ersten Spieler anlegen (automatisch Runner oder Hunter? z. B. Host = Hunter)
            var playerEntity = new PlayerEntity
            {
                Username = hostUsername,
                Role = PlayerRole.Hunter,
                LobbyId = lobbyEntity.LobbyId,
                IsReady = false,
                IsCaught = false
            };
            playerEntity = await _playerRepo.CreateAsync(playerEntity);

            lobbyEntity.PlayerIds.Add(playerEntity.PlayerId);
            await _lobbyRepo.UpdateAsync(lobbyEntity);

            // 4. DTO zusammensetzen
            return await MapToLobbyDtoAsync(lobbyEntity);
        }

        public async Task DeleteAsync(string lobbyId)
        {
            // TODO: alle abhängigen Daten löschen (Spieler, Settings, Votes)
            await _lobbyRepo.DeleteAsync(lobbyId);
        }

        public async Task<IEnumerable<LobbyDto>> GetAllLobbiesAsync()
        {
            var lobbies = await _lobbyRepo.GetAllAsync();
            var result = new List<LobbyDto>();
            foreach (var l in lobbies)
            {
                result.Add(await MapToLobbyDtoAsync(l));
            }
            return result;
        }

        public async Task<LobbyDto> GetLobbyByCodeAsync(string code)
        {
            var lobbyEntity = await _lobbyRepo.GetByCodeAsync(code);
            if (lobbyEntity == null)
                return null;
            return await MapToLobbyDtoAsync(lobbyEntity);
        }

        public async Task<LobbyDto> GetLobbyByIdAsync(string lobbyId)
        {
            var lobbyEntity = await _lobbyRepo.GetByIdAsync(lobbyId);
            if (lobbyEntity == null)
                throw new KeyNotFoundException("Lobby nicht gefunden");
            return await MapToLobbyDtoAsync(lobbyEntity);
        }

        public async Task JoinLobbyAsync(string lobbyId, string username, string userId)
        {
            var lobby = await _lobbyRepo.GetByIdAsync(lobbyId);
            if (lobby == null) throw new KeyNotFoundException("Lobby nicht gefunden");

            // Spieler nicht doppelt beitreten lassen
            var existing = (await _playerRepo.GetByLobbyIdAsync(lobbyId))
                           .FirstOrDefault(p => p.Username == username);
            if (existing != null)
                throw new InvalidOperationException("Benutzername bereits vergeben");

            var newPlayer = new PlayerEntity
            {
                Username = username,
                Role = PlayerRole.Runner, // z. B. standardmäßig Runner
                LobbyId = lobbyId,
                IsReady = false,
                IsCaught = false
            };
            newPlayer = await _playerRepo.CreateAsync(newPlayer);

            lobby.PlayerIds.Add(newPlayer.PlayerId);
            await _lobbyRepo.UpdateAsync(lobby);
        }

        public async Task LeaveLobbyAsync(string lobbyId, string playerId)
        {
            var lobby = await _lobbyRepo.GetByIdAsync(lobbyId);
            if (lobby == null) throw new KeyNotFoundException("Lobby nicht gefunden");

            // Entferne Spieler aus Lobby
            lobby.PlayerIds.Remove(playerId);
            await _lobbyRepo.UpdateAsync(lobby);

            // Lösche Player
            await _playerRepo.DeleteAsync(playerId);
        }

        public async Task UpdateLobbySettingsAsync(string lobbyId, SettingsDto newSettings, string requesterId)
        {
            var lobby = await _lobbyRepo.GetByIdAsync(lobbyId);
            if (lobby == null) throw new KeyNotFoundException("Lobby nicht gefunden");
            if (lobby.HostUserId != requesterId)
                throw new UnauthorizedAccessException("Nur der Host darf die Einstellungen ändern");

            var settingsEntity = await _settingsRepo.GetByIdAsync(lobby.SettingsId);
            if (settingsEntity == null) throw new KeyNotFoundException("Settings nicht gefunden");

            // Werte überschreiben
            settingsEntity.DisplayModeHunter = newSettings.DisplayModeHunter;
            settingsEntity.DisplayModeRunner = newSettings.DisplayModeRunner;
            settingsEntity.ShowDistance = newSettings.ShowDistance;
            settingsEntity.UseStartDelay = newSettings.UseStartDelay;
            settingsEntity.StartDelaySeconds = newSettings.StartDelaySeconds;
            settingsEntity.UseLocationDelay = newSettings.UseLocationDelay;
            settingsEntity.LocationDelaySeconds = newSettings.LocationDelaySeconds;
            settingsEntity.ManualRefresh = newSettings.ManualRefresh;
            settingsEntity.HunterVisibleToRunner = newSettings.HunterVisibleToRunner;
            settingsEntity.CompassPointsNextOpponent = newSettings.CompassPointsNextOpponent;

            await _settingsRepo.UpdateAsync(settingsEntity);
        }

        #region Private Hilfsmethoden

        private async Task<LobbyDto> MapToLobbyDtoAsync(LobbyEntity lobbyEntity)
        {
            // 1. Settings laden
            var settingsEntity = await _settingsRepo.GetByIdAsync(lobbyEntity.SettingsId);

            // 2. Spieler laden
            var players = await _playerRepo.GetByLobbyIdAsync(lobbyEntity.LobbyId);
            var playerDtos = players.Select(p => new PlayerDto
            {
                PlayerId = p.PlayerId,
                Username = p.Username,
                Role = p.Role,
                IsCaught = p.IsCaught,
                IsReady = p.IsReady,
                CurrentPosition = (p.CurrentLatitude.HasValue && p.CurrentLongitude.HasValue)
                    ? new PositionDto
                    {
                        PlayerId = p.PlayerId,
                        Latitude = p.CurrentLatitude.Value,
                        Longitude = p.CurrentLongitude.Value,
                        TimestampUtc = p.LastUpdateUtc ?? DateTime.UtcNow
                    }
                    : null,
                LastUpdateUtc = p.LastUpdateUtc
            }).ToList();

            // 3. DTO zusammenstellen
            return new LobbyDto
            {
                LobbyId = lobbyEntity.LobbyId,
                Code = lobbyEntity.Code,
                HostUserId = lobbyEntity.HostUserId,
                Status = lobbyEntity.Status,
                Settings = new SettingsDto
                {
                    DisplayModeHunter = settingsEntity.DisplayModeHunter,
                    DisplayModeRunner = settingsEntity.DisplayModeRunner,
                    ShowDistance = settingsEntity.ShowDistance,
                    UseStartDelay = settingsEntity.UseStartDelay,
                    StartDelaySeconds = settingsEntity.StartDelaySeconds,
                    UseLocationDelay = settingsEntity.UseLocationDelay,
                    LocationDelaySeconds = settingsEntity.LocationDelaySeconds,
                    ManualRefresh = settingsEntity.ManualRefresh,
                    HunterVisibleToRunner = settingsEntity.HunterVisibleToRunner,
                    CompassPointsNextOpponent = settingsEntity.CompassPointsNextOpponent
                },
                Players = playerDtos
            };
        }

        #endregion
    }
}
