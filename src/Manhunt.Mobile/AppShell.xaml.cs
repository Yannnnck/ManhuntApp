﻿using Manhunt.Mobile.Views;

namespace Manhunt.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Unterseiten (nicht in TabBar) registrieren
            Routing.RegisterRoute(nameof(JoinLobbyPage), typeof(JoinLobbyPage));
            Routing.RegisterRoute(nameof(CreateLobbyPage), typeof(CreateLobbyPage));
            Routing.RegisterRoute(nameof(GameSettingsPage), typeof(GameSettingsPage));
            Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
        }
    }


}
