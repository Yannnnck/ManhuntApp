// Datei: Manhunt.Backend/Models/JwtSettings.cs
namespace Manhunt.Backend.Models
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int TokenLifetimeMinutes { get; set; }
    }
}
