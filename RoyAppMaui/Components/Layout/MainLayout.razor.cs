using Microsoft.AspNetCore.Components;

using MudBlazor;

using RoyAppMaui.Services;

namespace RoyAppMaui.Components.Layout;
public partial class MainLayout
{
    [Inject] private NotifyStateService? Service { get; set; }

    private bool _isDarkMode;
    private MudThemeProvider _mudThemeProvider;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isDarkMode = await _mudThemeProvider.GetSystemPreference();
            await _mudThemeProvider.WatchSystemPreference(OnSystemPreferenceChanged);
            StateHasChanged();
        }
    }

    private void IncrementCount() =>
        Service.NotifyEventClick(this);

    private Task OnSystemPreferenceChanged(bool newValue)
    {
        _isDarkMode = newValue;
        StateHasChanged();
        return Task.CompletedTask;
    }
}
