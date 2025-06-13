using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manhunt.Shared.DTOs; // DTOs aus Shared-Library

namespace Manhunt.Mobile.Services
{
    public interface IApiService
    {
        Task<string> LoginAsync(string userId, string username);
        Task<List<LobbyDto>> GetAllLobbiesAsync();
        Task<LobbyDto> CreateLobbyAsync(CreateLobbyRequest req);
        Task JoinLobbyAsync(string code);
        Task<LobbyDto> GetLobbyByIdAsync(string lobbyId);
        // später: Task StartGameAsync(string lobbyId);
    }
}

