using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;

using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

using MudBlazor.Services;
using RoyAppMaui.Services;
using RoyAppMaui.Services.Impl;

namespace RoyAppMaui;

/// <summary>
/// Configures and builds the .NET MAUI application.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the app code behind. There's no logic to test.")]
public static class MauiProgram
{
    /// <summary>
    /// Creates and configures the MAUI app.
    /// </summary>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(ConfigureFonts)
            .ConfigureLifecycleEvents(ConfigureLifecycleEvents);

        // UI Services
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
            config.SnackbarConfiguration.ClearAfterNavigation = true;
        });

        // Platform/Toolkit Services
        builder.Services.AddSingleton(FileSaver.Default);
        builder.Services.AddSingleton(FilePicker.Default);
        builder.Services.AddSingleton<System.IO.Abstractions.IFileSystem, System.IO.Abstractions.FileSystem>();

        // App Services
        builder.Services.AddSingleton<ISettingsService>(new SettingsService(Preferences.Default));
        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<IImportExportService, ImportExportService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void ConfigureFonts(IFontCollection fonts)
    {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
    }

     private static void ConfigureLifecycleEvents(ILifecycleBuilder lifecycle)
    {
#if WINDOWS
        lifecycle.AddWindows(builder =>
        {
            builder.OnWindowCreated(window =>
            {
                window.Title = AppInfo.Current.Name;
            });
        });
#endif
    }
}
