using Microsoft.AspNetCore.Components;

using MudBlazor;

using RoyAppMaui.Enums;
using RoyAppMaui.Services;

namespace RoyAppMaui.Components.Layout;
public partial class MainLayout
{
    [Inject] private NotifyStateService NotifyService { get; set; } = default!;

    private bool _isDarkMode;
    private MudThemeProvider _mudThemeProvider = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isDarkMode = await _mudThemeProvider.GetSystemPreference();
            await _mudThemeProvider.WatchSystemPreference(OnSystemPreferenceChanged);
            StateHasChanged();
        }
    }

    private void HandleClearListClick() =>
        NotifyService.NotifyOnEventClick(this, new MenuItemClickEventArgs(MenuItems.Clear));

    private void HandleFileExportClick() =>
        NotifyService.NotifyOnEventClick(this, new MenuItemClickEventArgs(MenuItems.Export));

    private void HandleFileImportClick() =>
        NotifyService.NotifyOnEventClick(this, new MenuItemClickEventArgs(MenuItems.Import));

    private Task OnSystemPreferenceChanged(bool newValue)
    {
        _isDarkMode = newValue;
        StateHasChanged();
        return Task.CompletedTask;
    }
}
