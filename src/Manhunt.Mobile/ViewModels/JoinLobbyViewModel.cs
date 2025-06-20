using System.Threading.Tasks;
using System.Windows.Input;
using Manhunt.Mobile.Services;
using Microsoft.Maui.Controls;

namespace Manhunt.Mobile.ViewModels
{
    public class JoinLobbyViewModel : BaseViewModel
    {
        readonly IApiService _api;
        public string JoinCode { get; set; }

        public ICommand JoinCommand => new Command(async () => await JoinAsync());
        public ICommand BackCommand => new Command(() => Shell.Current.GoToAsync(".."));

        public JoinLobbyViewModel(IApiService api) => _api = api;

        async Task JoinAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                // hier später: _api.JoinLobbyAsync(JoinCode);
                await Task.Delay(500);
                await Shell.Current.GoToAsync($"CreateLobbyPage"); // z.B. weiter
            }
            catch (System.Exception ex) { Error = ex.Message; }
            finally { IsBusy = false; }
        }
    }
}
