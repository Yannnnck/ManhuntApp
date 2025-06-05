using Manhunt.Shared.Enums;

namespace Manhunt.Backend.Models.Requestsplayer
{
    public class PlayerCreatePlayerRequest
    {
        public string LobbyId { get; set; }
        public string Username { get; set; }
        public PlayerRole Role { get; set; }
    }
}
