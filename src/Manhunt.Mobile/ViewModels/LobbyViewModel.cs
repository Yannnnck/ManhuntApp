using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Manhunt.Mobile.Services;
using Manhunt.Shared.DTOs;
using Microsoft.Maui.Controls;

namespace Manhunt.Mobile.ViewModels
{
    public class LobbyViewModel : BaseViewModel
    {
        readonly IApiService _api;
        public ObservableCollection<LobbyDto> Lobbies { get; } = new();

        public ICommand GoToJoin => new Command(() => Shell.Current.GoToAsync("JoinLobbyPage"));
        public ICommand GoToCreate => new Command(() => Shell.Current.GoToAsync("CreateLobbyPage"));

        public LobbyViewModel(IApiService api)
        {
            _api = api;
            Task.Run(LoadLobbies);
        }

        async Task LoadLobbies()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var list = await _api.GetAllLobbiesAsync();
                Lobbies.Clear();
                foreach (var l in list) Lobbies.Add(l);
            }
            catch (System.Exception ex) { Error = ex.Message; }
            finally { IsBusy = false; }
        }
    }
}

