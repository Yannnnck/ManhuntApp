// Datei: Manhunt.Backend/Models/Requestsvote/VoteStartVoteRequest.cs
using Manhunt.Shared.Enums;

namespace Manhunt.Backend.Models.Requestsvote
{
    public class VoteStartVoteRequest
    {
        public string LobbyId { get; set; }
        public RequestType RequestType { get; set; }
        public string RequesterId { get; set; }
    }
}
