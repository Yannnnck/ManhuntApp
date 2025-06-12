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

        [BsonElement("Code")]
        public string Code { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("SettingsId")]
        public string SettingsId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("hostUserId")]
        public string HostUserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("PlayerIds")]
        public List<string> PlayerIds { get; set; } = new();

        [BsonElement("Status")]
        public LobbyStatus Status { get; set; } = LobbyStatus.Created;
    }
}
