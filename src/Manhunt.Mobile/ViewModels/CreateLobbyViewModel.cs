using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Input;
using Manhunt.Mobile.Services;
using Manhunt.Shared.DTOs;
using Microsoft.Maui.Controls;

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
            try
            {
                var req = new CreateLobbyRequest
                {
                    HostUserId = /* hier aus Token / Claim */,
                    HostUsername = /* hier aus Claim */,
                    InitialSettings = InitialSettings
                };
                var lobby = await _api.CreateLobbyAsync(req);
                await Shell.Current.GoToAsync($"GameSettingsPage?lobbyId={lobby.LobbyId}");
            }
            catch (System.Exception ex) { Error = ex.Message; }
            finally { IsBusy = false; }
        }
    }
}

