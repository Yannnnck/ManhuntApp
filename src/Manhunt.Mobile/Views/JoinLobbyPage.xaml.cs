namespace Manhunt.Mobile.Views;
using Manhunt.Mobile.ViewModels;

public partial class JoinLobbyPage : ContentPage
{
	public JoinLobbyPage(JoinLobbyViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}