using Manhunt.Shared.DTOs;

namespace Manhunt.Backend.Models.Requestslobby
{
    public class LobbyCreateLobbyRequest
    {
        public string HostUserId { get; set; }
        public string HostUsername { get; set; }
        public SettingsDto InitialSettings { get; set; }
    }
}
