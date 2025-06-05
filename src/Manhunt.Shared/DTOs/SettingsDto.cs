using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Datei: Manhunt.Shared/DTOs/SettingsDto.cs
namespace Manhunt.Shared.DTOs
{
    public class SettingsDto
    {
        // Anzeige-Optionen
        public DisplayMode DisplayModeHunter { get; set; }  // Compass, Map, Both
        public DisplayMode DisplayModeRunner { get; set; }  // Compass, Map, Both
        public bool ShowDistance { get; set; }              // Ja/Nein

        // Verzögerungs-Optionen
        public bool UseStartDelay { get; set; }
        public int StartDelaySeconds { get; set; }           // 0–300
        public bool UseLocationDelay { get; set; }
        public int LocationDelaySeconds { get; set; }        // 0–1200

        // Sonstige Optionen
        public bool ManualRefresh { get; set; }
        public bool HunterVisibleToRunner { get; set; }
        public bool CompassPointsNextOpponent { get; set; }
    }

    // Hilfs-Enum für Anzeige-Modus
    public enum DisplayMode
    {
        Compass,
        Map,
        Both
    }
}

