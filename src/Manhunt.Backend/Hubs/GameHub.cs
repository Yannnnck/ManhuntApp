// Datei: Manhunt.Backend/Hubs/GameHub.cs
using Manhunt.Backend.Services.Interfaces;
using Manhunt.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Manhunt.Backend.Hubs
{
    [Authorize] // Nur authentifizierte Benutzer dürfen beitreten
    public class GameHub : Hub
    {
        private readonly IPlayerService _playerService;
        private readonly ILobbyService _lobbyService;
        private readonly IGameService _gameService;

        public GameHub(
            IPlayerService playerService,
            ILobbyService lobbyService,
            IGameService gameService)
        {
            _playerService = playerService;
            _lobbyService = lobbyService;
            _gameService = gameService;
        }

        // Client ruft diese Methode auf, um sich einer Lobby (SignalR-Gruppe) anzuschließen
        public async Task JoinLobbyGroup(string lobbyId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
        }

        // Client verlässt die Lobby-Gruppe
        public async Task LeaveLobbyGroup(string lobbyId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId);
        }

        /// <summary>
        /// Der Client sendet seine Positionsdaten – der Server speichert sie 
        /// und broadcastet die aktualisierte Position an alle anderen in der Gruppe.
        /// </summary>
        public async Task UpdatePosition(PositionDto position)
        {
            // 1. Service: Positionsdaten abspeichern
            await _playerService.UpdatePlayerPositionAsync(position);

            // 2. Broadcast: An alle anderen in derselben Lobby senden
            await Clients.Group($"Lobby-{position.PlayerId}") // oder einen sinnvollen Gruppennamen: z. B. lobbyId
                  .SendAsync("ReceivePositionUpdate", position);
        }

        /// <summary>
        /// Client signalisiert, dass er Ready ist oder nicht; Broadcast an alle
        /// </summary>
        public async Task UpdateReadyStatus(string playerId, bool isReady)
        {
            await _playerService.MarkPlayerReadyAsync(playerId, isReady);
            // Hole LobbyId, um an richtige Gruppe zu senden – Beispiel: Wir nehmen an, es gibt eine Methode:
            var player = await _playerService.GetPlayerByIdAsync(playerId);
            if (player == null) return;

            // Sende an Gruppe "Lobby-[lobbyId]"
            await Clients.Group($"Lobby-{player.PlayerId}") // ggf. durch lobbyId ersetzen
                  .SendAsync("ReceiveReadyStatusUpdate", playerId, isReady);
        }

        /// <summary>
        /// Host markiert Runner als gefangen. Broadcast an alle
        /// </summary>
        public async Task MarkCaught(string lobbyId, string playerId)
        {
            // Überprüfen intern, ob Caller Host ist, etc.
            await _playerService.MarkPlayerCaughtAsync(lobbyId, playerId, Context.UserIdentifier);

            await Clients.Group($"Lobby-{lobbyId}")
                 .SendAsync("ReceiveCaughtNotification", playerId);
        }

        /// <summary>
        /// Host startet das Spiel (Runner auswählen + Lobby-Status setzen). Broadcast an Gruppe.
        /// </summary>
        public async Task StartGame(string lobbyId, string[] runnerIds)
        {
            await _gameService.StartGameAsync(lobbyId, Context.UserIdentifier, runnerIds);

            await Clients.Group($"Lobby-{lobbyId}")
                  .SendAsync("ReceiveGameStarted", lobbyId, runnerIds);
        }

        /// <summary>
        /// Host beendet das Spiel. Broadcast.
        /// </summary>
        public async Task EndGame(string lobbyId)
        {
            await _gameService.EndGameAsync(lobbyId, Context.UserIdentifier);
            await Clients.Group($"Lobby-{lobbyId}")
                  .SendAsync("ReceiveGameEnded", lobbyId);
        }

        // Weitere Hub-Methoden: PauseGame, VoteStart, CastVote, etc.
    }
}
