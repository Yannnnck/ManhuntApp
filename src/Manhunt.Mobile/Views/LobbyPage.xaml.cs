using System.Windows.Input;

namespace Manhunt.Mobile.Views;

public partial class LobbyPage : ContentPage
{
	public LobbyPage()
	{
		InitializeComponent();
	}
    public ICommand GoToJoin => new Command(() => Shell.Current.GoToAsync("JoinLobbyPage"));
    public ICommand GoToCreate => new Command(() => Shell.Current.GoToAsync("CreateLobbyPage"));
}