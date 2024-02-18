using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;

using Microsoft.Extensions.Logging;

using MudBlazor;
using MudBlazor.Services;
using RoyAppMaui.Services;
using RoyAppMaui.Services.Impl;

namespace RoyAppMaui;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
            config.SnackbarConfiguration.ClearAfterNavigation = true;
        });
        builder.Services.AddSingleton(FileSaver.Default);
        builder.Services.AddSingleton<ISettingsService>(new SettingsService(Preferences.Default));

        builder.Services.AddSingleton<IDataService, DataService>();
        builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddScoped<NotifyService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
