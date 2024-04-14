using RoyAppMaui.Services;

namespace RoyAppMaui.Components.Layout;
public partial class MainLayout
{
    [Inject] private ISettingsService Settings { get; set; } = default!;

    private MudThemeProvider _mudThemeProvider = default!;

    private string ThemeIcon => Settings.IsDarkMode
                             ? Icons.Material.Outlined.DarkMode
                             : Icons.Material.Outlined.LightMode;
    private MudBlazor.Color ThemeIconColor => Settings.IsDarkMode
                                           ? MudBlazor.Color.Inherit
                                           : MudBlazor.Color.Warning;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await _mudThemeProvider.WatchSystemPreference(OnSystemPreferenceChanged);
            StateHasChanged();
        }
    }

    private Task OnSystemPreferenceChanged(bool newValue)
    {
        Settings.IsDarkMode = newValue;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private void ThemeToggle() =>
        Settings.IsDarkMode = !Settings.IsDarkMode;
}
