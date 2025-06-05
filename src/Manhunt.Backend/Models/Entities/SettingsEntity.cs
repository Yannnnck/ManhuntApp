// Datei: Manhunt.Backend/Models/Entities/SettingsEntity.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Manhunt.Shared.DTOs;

namespace Manhunt.Backend.Models.Entities
{
    public class SettingsEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SettingsId { get; set; }

        // Anzeige-Optionen
        [BsonElement("DisplayModeHunter")]
        public DisplayMode DisplayModeHunter { get; set; }

        [BsonElement("DisplayModeRunner")]
        public DisplayMode DisplayModeRunner { get; set; }

        [BsonElement("ShowDistance")]
        public bool ShowDistance { get; set; }

        // Verzögerungs-Optionen
        [BsonElement("UseStartDelay")]
        public bool UseStartDelay { get; set; }

        [BsonElement("StartDelaySeconds")]
        public int StartDelaySeconds { get; set; }

        [BsonElement("UseLocationDelay")]
        public bool UseLocationDelay { get; set; }

        [BsonElement("LocationDelaySeconds")]
        public int LocationDelaySeconds { get; set; }

        // Sonstige Optionen
        [BsonElement("ManualRefresh")]
        public bool ManualRefresh { get; set; }

        [BsonElement("HunterVisibleToRunner")]
        public bool HunterVisibleToRunner { get; set; }

        [BsonElement("CompassPointsNextOpponent")]
        public bool CompassPointsNextOpponent { get; set; }
    }
}
