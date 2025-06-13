using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhunt.Mobile.ViewModels
{
    [QueryProperty(nameof(LobbyId), "lobbyId")]
    public class GameSettingsViewModel : BaseViewModel
    {
        string _lobbyId;
        public string LobbyId
        {
            get => _lobbyId; set
            {
                SetProperty(ref _lobbyId, value);
                LoadLobby();
            }
        }

        async void LoadLobby() { /* _api.GetLobbyByIdAsync(LobbyId) … */ }
  …
}

}
