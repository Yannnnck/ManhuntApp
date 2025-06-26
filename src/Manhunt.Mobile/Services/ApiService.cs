using Manhunt.Shared.Models.Requests.Manhunt.Shared.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Manhunt.Shared.DTOs;
using Manhunt.Mobile.Helpers;
using System.Text.Json;

namespace Manhunt.Mobile.Services
{
    public class ApiService : IApiService
    {
        readonly HttpClient _client;
        public ApiService(HttpClient client) => _client = client;

        public async Task<string> LoginAsync(string userId, string username)
        {
            var res = await _client.PostAsJsonAsync("api/Auth/login", new { userId, username });
            res.EnsureSuccessStatusCode();
            var obj = await res.Content.ReadFromJsonAsync<JsonElement>();
            var token = obj.GetProperty("token").GetString();
            TokenStorage.Set(token);
            _client.DefaultRequestHeaders.Authorization =
              new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return token;
        }

        public Task<List<LobbyDto>> GetAllLobbiesAsync() =>
          _client.GetFromJsonAsync<List<LobbyDto>>("api/Lobby");

        public Task<LobbyDto> CreateLobbyAsync(CreateLobbyRequest req) =>
          _client.PostAsJsonAsync("api/Lobby", req)
                 .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<LobbyDto>())
                 .Unwrap();

        public async Task JoinLobbyAsync(string code)
        {
            var res = await _client.PostAsJsonAsync("api/Lobby/join", new { code });
            res.EnsureSuccessStatusCode();
        }

        public Task<LobbyDto> GetLobbyByIdAsync(string lobbyId) =>
          _client.GetFromJsonAsync<LobbyDto>($"api/Lobby/{lobbyId}");
    }
}

