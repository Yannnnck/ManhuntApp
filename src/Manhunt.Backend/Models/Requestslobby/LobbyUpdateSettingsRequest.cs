// Datei: Manhunt.Backend/Models/Requestslobby/LobbyUpdateSettingsRequest.cs
using Manhunt.Shared.DTOs;

namespace Manhunt.Backend.Models.Requestslobby
{
    public class LobbyUpdateSettingsRequest
    {
        public string LobbyId { get; set; }
        public SettingsDto NewSettings { get; set; }
        public string RequesterId { get; set; }
    }
}
