// Datei: Manhunt.Backend/Models/Requestsplayer/PlayerMarkCaughtRequest.cs
namespace Manhunt.Backend.Models.Requestsplayer
{
    public class PlayerMarkCaughtRequest
    {
        public string LobbyId { get; set; }
        public string PlayerId { get; set; }
        public string RequesterId { get; set; }
    }

}
