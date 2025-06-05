using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manhunt.Shared.Enums;

// Datei: Manhunt.Shared/DTOs/LobbyDto.cs
namespace Manhunt.Shared.DTOs
{
    public class LobbyDto
    {
        public string LobbyId { get; set; }            // MongoDB ObjectId als String
        public string Code { get; set; }               // Einmaliger Join-Code (z. B. "ABCD")
        public SettingsDto Settings { get; set; }      // Referenz auf die Einstellungen
        public string HostUserId { get; set; }         // Wer hat die Lobby erstellt
        public List<PlayerDto> Players { get; set; }   // Aktuelle Spieler in der Lobby
        public LobbyStatus Status { get; set; }        // Created, Running, Paused, Ended
    }
}

