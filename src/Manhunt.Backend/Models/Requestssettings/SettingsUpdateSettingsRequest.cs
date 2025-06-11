// Datei: Manhunt.Backend/Models/Requestssettings/SettingsUpdateSettingsRequest.cs
using Manhunt.Shared.DTOs;

namespace Manhunt.Backend.Models.Requestssettings
{
    public class SettingsUpdateSettingsRequest
    {
        public string RequesterId { get; set; }
        public SettingsDto NewSettings { get; set; }
    }
}
