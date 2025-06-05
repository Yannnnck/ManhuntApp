using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manhunt.Shared.Enums;

// Datei: Manhunt.Shared/DTOs/VoteDto.cs
namespace Manhunt.Shared.DTOs
{
    public class VoteDto
    {
        public string LobbyId { get; set; }
        public string PlayerId { get; set; }
        public bool VoteFor { get; set; }       // true = Ja, false = Nein
        public RequestType RequestType { get; set; } // Restart oder EndGame
    }
}

