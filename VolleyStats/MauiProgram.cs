using Microsoft.Extensions.Logging;
using VolleyStats.Services;
using VolleyStats.ViewModels;
using VolleyStats.Views;

namespace VolleyStats;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<MatchesPageViewModel>();
        builder.Services.AddSingleton<PlayersPageViewModel>();
        builder.Services.AddSingleton<StandingsPageViewModel>();
        builder.Services.AddTransient<PlayerProfilePageViewModel>();
        builder.Services.AddSingleton<MatchesPage>();
        builder.Services.AddSingleton<PlayersPage>();
        builder.Services.AddSingleton<StandingsPage>();
        builder.Services.AddTransient<PlayerProfilePage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
