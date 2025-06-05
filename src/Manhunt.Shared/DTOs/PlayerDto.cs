using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manhunt.Shared.Enums;

// Datei: Manhunt.Shared/DTOs/PlayerDto.cs
namespace Manhunt.Shared.DTOs
{
    public class PlayerDto
    {
        public string PlayerId { get; set; }            // MongoDB ObjectId als String
        public string Username { get; set; }
        public PlayerRole Role { get; set; }            // Hunter oder Runner
        public bool IsCaught { get; set; }              // Für Runner: gefangen oder nicht
        public bool IsReady { get; set; }               // Ready-Button
        public PositionDto CurrentPosition { get; set; } // Aktuelle Position (kann null sein)
        public DateTime? LastUpdateUtc { get; set; }    // Letztes GPS-Update
    }
}

