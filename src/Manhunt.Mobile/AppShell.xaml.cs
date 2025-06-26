using Manhunt.Mobile.Views;

namespace Manhunt.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // alle Unter-Routen registrieren, mit denen wir per GoToAsync arbeiten wollen
            Routing.RegisterRoute(nameof(GameSettingsPage), typeof(GameSettingsPage));
            Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
        }
    }

}
