using MudBlazor;

namespace RoyAppMaui.Components.Layout;
public partial class MainLayout
{
    private bool _drawerOpen = false;
    private bool _isDarkMode;
    private MudThemeProvider _mudThemeProvider;

    private void DrawerToggle() =>
        _drawerOpen = !_drawerOpen;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isDarkMode = await _mudThemeProvider.GetSystemPreference();
            await _mudThemeProvider.WatchSystemPreference(OnSystemPreferenceChanged);
            StateHasChanged();
        }
    }

    private Task OnSystemPreferenceChanged(bool newValue)
    {
        _isDarkMode = newValue;
        StateHasChanged();
        return Task.CompletedTask;
    }
}
