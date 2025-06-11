// Datei: Manhunt.Backend/Models/Requestslobby/LobbyLeaveLobbyRequest.cs
using Manhunt.Shared.DTOs;

namespace Manhunt.Backend.Models.Requestslobby
{
    public class LobbyLeaveLobbyRequest
    {
        public string LobbyId { get; set; }
        public string PlayerId { get; set; }
    }
}
