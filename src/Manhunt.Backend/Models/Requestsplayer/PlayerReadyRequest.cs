// Datei: Manhunt.Backend/Models/Requestsplayer/PlayerReadyRequest.cs
namespace Manhunt.Backend.Models.Requestsplayer
{
    public class PlayerReadyRequest
    {
        public string PlayerId { get; set; }
        public bool IsReady { get; set; }
    }
}
