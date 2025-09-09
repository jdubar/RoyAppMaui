using Microsoft.JSInterop;

using RoyAppMaui.Services;
using RoyAppMaui.Theme;

namespace RoyAppMaui.Components.Layout;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class MainLayout
{
    [Inject] private IImportExportService ImportExportService { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private ISettingsService SettingsService { get; set; } = default!;

    private CustomTheme CustomTheme { get; } = new();
    private MudThemeProvider _mudThemeProvider = default!;

    private string ThemeIcon => SettingsService.IsDarkMode
                             ? Icons.Material.Outlined.DarkMode
                             : Icons.Material.Outlined.LightMode;
    private MudBlazor.Color ThemeIconColor => SettingsService.IsDarkMode
                                           ? MudBlazor.Color.Inherit
                                           : MudBlazor.Color.Warning;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && _mudThemeProvider is not null)
        {
            await _mudThemeProvider.WatchSystemDarkModeAsync(OnSystemPreferenceChanged);
            StateHasChanged();
        }
    }

    private void OnExportDataClick() => ImportExportService.RequestExport();

    private void OnImportDataClick() => ImportExportService.RequestImport();

    private Task OnSystemPreferenceChanged(bool newValue)
    {
        SettingsService.IsDarkMode = newValue;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task ThemeToggleAsync()
    {
        SettingsService.IsDarkMode = !SettingsService.IsDarkMode;
        await JS.InvokeVoidAsync("setIsDarkModeCookie", SettingsService.IsDarkMode);
    }
}
