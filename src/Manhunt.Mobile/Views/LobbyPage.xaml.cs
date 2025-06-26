using System.Windows.Input;
using Manhunt.Mobile.ViewModels;

namespace Manhunt.Mobile.Views;

public partial class LobbyPage : ContentPage
{
	public LobbyPage(LobbyViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
    public ICommand GoToJoin => new Command(() => Shell.Current.GoToAsync("JoinLobbyPage"));
    public ICommand GoToCreate => new Command(() => Shell.Current.GoToAsync("CreateLobbyPage"));
}