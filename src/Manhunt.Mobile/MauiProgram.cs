using Microsoft.Extensions.Logging;
using Manhunt.Mobile.Helpers;
using Manhunt.Mobile.Services;
using Manhunt.Mobile.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Manhunt.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            TokenStorage.Load();

            var builder = MauiApp.CreateBuilder();
            builder
              .UseMauiApp<App>()
              .ConfigureFonts(fonts =>
              {
                  fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
              });

            // HttpClient + Bearer
            builder.Services.AddHttpClient<IApiService, ApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7032/");
                if (!string.IsNullOrWhiteSpace(TokenStorage.Token))
                    client.DefaultRequestHeaders.Authorization =
                      new AuthenticationHeaderValue("Bearer", TokenStorage.Token);
            });

            // ViewModels
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<LobbyViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddTransient<JoinLobbyViewModel>();
            builder.Services.AddTransient<CreateLobbyViewModel>();
            builder.Services.AddTransient<GameSettingsViewModel>();
            builder.Services.AddTransient<GameViewModel>();

            return builder.Build();
        }
    }


}
