// Datei: Manhunt.Backend/Models/Requestslobby/LobbyJoinLobbyRequest.cs
using Manhunt.Shared.DTOs;

namespace Manhunt.Backend.Models.Requestslobby
{
    public class LobbyJoinLobbyRequest
    {
        public string LobbyId { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
    }
}
