namespace Manhunt.Backend.Models.Requestsplayer
{
    public class PlayerReadyRequest
    {
        public string PlayerId { get; set; }
        public bool IsReady { get; set; }
    }
}
