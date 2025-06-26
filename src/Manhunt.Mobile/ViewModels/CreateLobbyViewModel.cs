using Manhunt.Shared.Models.Requests.Manhunt.Shared.Models.Requests;
using Manhunt.Mobile.Helpers;
using Manhunt.Mobile.Services;
using Manhunt.Shared.DTOs;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IdentityModel.Tokens.Jwt;



namespace Manhunt.Mobile.ViewModels
{
    public class CreateLobbyViewModel : BaseViewModel
    {
        readonly IApiService _api;
        public SettingsDto InitialSettings { get; set; } = new();
        public ICommand CreateCommand => new Command(async () => await CreateAsync());
        public ICommand BackCommand => new Command(() => Shell.Current.GoToAsync(".."));

        public CreateLobbyViewModel(IApiService api) => _api = api;

        async Task CreateAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            Error = string.Empty;

            try
            {
                // 1) Token holen
                var jwt = TokenStorage.GetToken();
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);

                // 2) Claims auslesen
                var userId = token.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var username = token.Claims.First(c => c.Type == ClaimTypes.Name).Value;

                // 3) Request bauen – hier (!) InitialSettings statt initialSettings
                var req = new CreateLobbyRequest
                {
                    HostUserId = userId,
                    HostUsername = username,
                    InitialSettings = InitialSettings
                };

                // 4) API-Aufruf
                var lobby = await _api.CreateLobbyAsync(req);

                // 5) Navigation
                await Shell.Current.GoToAsync($"GameSettingsPage?lobbyId={lobby.LobbyId}");
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

