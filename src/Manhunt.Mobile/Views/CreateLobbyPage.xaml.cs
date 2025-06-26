namespace Manhunt.Mobile.Views;
using Manhunt.Mobile.ViewModels;

public partial class CreateLobbyPage : ContentPage
{
    public CreateLobbyPage(CreateLobbyViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}