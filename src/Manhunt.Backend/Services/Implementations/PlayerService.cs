// Datei: Manhunt.Backend/Services/Implementations/PlayerService.cs
using Manhunt.Backend.Models.Entities;
using Manhunt.Backend.Repositories.Interfaces;
using Manhunt.Backend.Services.Interfaces;
using Manhunt.Shared.DTOs;
using Manhunt.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manhunt.Backend.Services.Implementations
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly ILobbyRepository _lobbyRepo;

        public PlayerService(
            IPlayerRepository playerRepo,
            ILobbyRepository lobbyRepo)
        {
            _playerRepo = playerRepo;
            _lobbyRepo = lobbyRepo;
        }

        public async Task<PlayerDto> CreatePlayerAsync(string lobbyId, string username, PlayerRole role)
        {
            var lobby = await _lobbyRepo.GetByIdAsync(lobbyId);
            if (lobby == null)
                throw new KeyNotFoundException("Lobby nicht gefunden");

            var newPlayer = new PlayerEntity
            {
                Username = username,
                Role = role,
                LobbyId = lobbyId,
                IsReady = false,
                IsCaught = false
            };
            newPlayer = await _playerRepo.CreateAsync(newPlayer);

            // Lobby updaten
            lobby.PlayerIds.Add(newPlayer.PlayerId);
            await _lobbyRepo.UpdateAsync(lobby);

            return new PlayerDto
            {
                PlayerId = newPlayer.PlayerId,
                Username = newPlayer.Username,
                Role = newPlayer.Role,
                IsCaught = newPlayer.IsCaught,
                IsReady = newPlayer.IsReady,
                CurrentPosition = null,
                LastUpdateUtc = null
            };
        }

        public async Task DeletePlayerAsync(string lobbyId, string playerId, string requesterId)
        {
            var lobby = await _lobbyRepo.GetByIdAsync(lobbyId);
            if (lobby == null)
                throw new KeyNotFoundException("Lobby nicht gefunden");

            // Nur Host darf kicken
            if (lobby.HostUserId != requesterId)
                throw new UnauthorizedAccessException("Nur Host darf Spieler entfernen");

            // Aus Lobby entfernen
            lobby.PlayerIds.Remove(playerId);
            await _lobbyRepo.UpdateAsync(lobby);

            // Player­Dokument löschen
            await _playerRepo.DeleteAsync(playerId);
        }

        public async Task<PlayerDto> GetPlayerByIdAsync(string playerId)
        {
            var p = await _playerRepo.GetByIdAsync(playerId);
            if (p == null)
                return null;

            return new PlayerDto
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
            };
        }

        public async Task<IEnumerable<PlayerDto>> GetPlayersByLobbyIdAsync(string lobbyId)
        {
            var players = await _playerRepo.GetByLobbyIdAsync(lobbyId);
            return players.Select(p => new PlayerDto
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
            });
        }

        public async Task MarkPlayerCaughtAsync(string lobbyId, string playerId, string requesterId)
        {
            var lobby = await _lobbyRepo.GetByIdAsync(lobbyId);
            if (lobby == null)
                throw new KeyNotFoundException("Lobby nicht gefunden");

            // Nur Host darf Runner als gefangen markieren
            if (lobby.HostUserId != requesterId)
                throw new UnauthorizedAccessException("Nur Host darf Runner als gefangen markieren");

            var player = await _playerRepo.GetByIdAsync(playerId);
            if (player == null)
                throw new KeyNotFoundException("Spieler nicht gefunden");

            player.IsCaught = true;
            await _playerRepo.UpdateAsync(player);
        }

        public async Task MarkPlayerReadyAsync(string playerId, bool isReady)
        {
            var player = await _playerRepo.GetByIdAsync(playerId);
            if (player == null)
                throw new KeyNotFoundException("Spieler nicht gefunden");

            player.IsReady = isReady;
            await _playerRepo.UpdateAsync(player);
        }

        public async Task UpdatePlayerPositionAsync(PositionDto position)
        {
            var player = await _playerRepo.GetByIdAsync(position.PlayerId);
            if (player == null)
                throw new KeyNotFoundException("Spieler nicht gefunden");

            player.CurrentLatitude = position.Latitude;
            player.CurrentLongitude = position.Longitude;
            player.LastUpdateUtc = position.TimestampUtc;

            await _playerRepo.UpdateAsync(player);
        }
    }
}
