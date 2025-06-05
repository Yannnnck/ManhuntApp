// Datei: Manhunt.Backend/Services/Implementations/GameService.cs
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
    public class GameService : IGameService
    {
        private readonly ILobbyRepository _lobbyRepo;
        private readonly IPlayerRepository _playerRepo;
        // Wenn nötig: IVoteRepository etc.

        public GameService(
            ILobbyRepository lobbyRepo,
            IPlayerRepository playerRepo)
        {
            _lobbyRepo = lobbyRepo;
            _playerRepo = playerRepo;
        }

        public async Task EndGameAsync(string lobbyId, string requesterId)
        {
            var lobby = await _lobbyRepo.GetByIdAsync(lobbyId);
            if (lobby == null) throw new KeyNotFoundException("Lobby nicht gefunden");
            if (lobby.HostUserId != requesterId) throw new UnauthorizedAccessException("Nur Host darf Spiel beenden");

            lobby.Status = LobbyStatus.Ended;
            await _lobbyRepo.UpdateAsync(lobby);
        }

        public async Task StartGameAsync(string lobbyId, string requesterId, IEnumerable<string> runnerIds)
        {
            var lobby = await _lobbyRepo.GetByIdAsync(lobbyId);
            if (lobby == null) throw new KeyNotFoundException("Lobby nicht gefunden");
            if (lobby.HostUserId != requesterId) throw new UnauthorizedAccessException("Nur Host darf Spiel starten");

            // Runner rollen zuweisen
            var allPlayers = (await _playerRepo.GetByLobbyIdAsync(lobbyId)).ToList();
            foreach (var p in allPlayers)
            {
                p.Role = runnerIds.Contains(p.PlayerId) ? PlayerRole.Runner : PlayerRole.Hunter;
                p.IsCaught = false;
                p.IsReady = false;
                await _playerRepo.UpdateAsync(p);
            }

            lobby.Status = LobbyStatus.Running;
            await _lobbyRepo.UpdateAsync(lobby);
        }

        public async Task PauseGameAsync(string lobbyId, string requesterId)
        {
            var lobby = await _lobbyRepo.GetByIdAsync(lobbyId);
            if (lobby == null) throw new KeyNotFoundException("Lobby nicht gefunden");
            if (lobby.HostUserId != requesterId) throw new UnauthorizedAccessException("Nur Host darf Spiel pausieren");

            lobby.Status = lobby.Status == LobbyStatus.Running ? LobbyStatus.Paused : LobbyStatus.Running;
            await _lobbyRepo.UpdateAsync(lobby);
        }

        public async Task<IEnumerable<PositionDto>> GetAllPositionsAsync(string lobbyId)
        {
            var players = await _playerRepo.GetByLobbyIdAsync(lobbyId);
            return players
                .Where(p => p.CurrentLatitude.HasValue && p.CurrentLongitude.HasValue)
                .Select(p => new PositionDto
                {
                    PlayerId = p.PlayerId,
                    Latitude = p.CurrentLatitude.Value,
                    Longitude = p.CurrentLongitude.Value,
                    TimestampUtc = p.LastUpdateUtc ?? DateTime.UtcNow
                });
        }

        public async Task SelectRunnersAsync(string lobbyId, IEnumerable<string> runnerIds, string requesterId)
        {
            var lobby = await _lobbyRepo.GetByIdAsync(lobbyId);
            if (lobby == null) throw new KeyNotFoundException("Lobby nicht gefunden");
            if (lobby.HostUserId != requesterId) throw new UnauthorizedAccessException("Nur Host darf Runner auswählen");

            var allPlayers = (await _playerRepo.GetByLobbyIdAsync(lobbyId)).ToList();
            foreach (var p in allPlayers)
            {
                p.Role = runnerIds.Contains(p.PlayerId) ? PlayerRole.Runner : PlayerRole.Hunter;
                p.IsCaught = false;
                p.IsReady = false;
                await _playerRepo.UpdateAsync(p);
            }
        }
    }
}
