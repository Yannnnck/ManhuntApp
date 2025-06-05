// Datei: Manhunt.Backend/Models/Entities/PlayerEntity.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using Manhunt.Shared.Enums;

namespace Manhunt.Backend.Models.Entities
{
    public class PlayerEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PlayerId { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("Role")]
        public PlayerRole Role { get; set; }

        [BsonElement("IsCaught")]
        public bool IsCaught { get; set; }

        [BsonElement("IsReady")]
        public bool IsReady { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("LobbyId")]
        public string LobbyId { get; set; }

        [BsonElement("CurrentLatitude")]
        public double? CurrentLatitude { get; set; }

        [BsonElement("CurrentLongitude")]
        public double? CurrentLongitude { get; set; }

        [BsonElement("LastUpdateUtc")]
        public DateTime? LastUpdateUtc { get; set; }
    }
}
