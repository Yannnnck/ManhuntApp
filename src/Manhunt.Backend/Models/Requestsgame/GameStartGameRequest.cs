namespace Manhunt.Backend.Models.Requestsgame
{
    public class GameStartGameRequest
    {
        public string LobbyId { get; set; }
        public string RequesterId { get; set; }
        public IEnumerable<string> RunnerIds { get; set; }
    }
}
