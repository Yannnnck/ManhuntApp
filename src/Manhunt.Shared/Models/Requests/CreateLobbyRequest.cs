using Manhunt.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhunt.Shared.Models.Requests
{
    namespace Manhunt.Shared.Models.Requests
    {
        public class CreateLobbyRequest
        {
            public string HostUserId { get; set; }
            public string HostUsername { get; set; }
            public SettingsDto InitialSettings { get; set; }
        }
    }

}
