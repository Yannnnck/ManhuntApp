// Datei: Manhunt.Backend/Models/Entities/LobbyEntity.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using Manhunt.Shared.Enums;

namespace Manhunt.Backend.Models.Entities
{
    public class LobbyEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string LobbyId { get; set; }

        // kein Attribut nötig, Default wäre "code"
        public string Code { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("settingsId")]
        public string SettingsId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("HostId")]
        public string HostUserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("PlayersIds")]
        public List<string> PlayerIds { get; set; } = new();

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("PlayersId")]
        public List<string> PlayerId { get; set; } = new();

        public LobbyStatus Status { get; set; } = LobbyStatus.Created;
    }

}
