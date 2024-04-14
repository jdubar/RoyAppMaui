using Microsoft.AspNetCore.Components;

using RoyAppMaui.Enums;
using RoyAppMaui.Services;

namespace RoyAppMaui.Components.Layout;
public partial class MainMenu
{
    [Inject] private NotifyService NotifyService { get; set; } = default!;

    private void HandleClearListClick() =>
        NotifyService.NotifyOnEventClick(this, new MenuItemClickEventArgs(MenuItems.Clear));

    private void HandleFileExportClick() =>
        NotifyService.NotifyOnEventClick(this, new MenuItemClickEventArgs(MenuItems.Export));

    private void HandleFileImportClick() =>
        NotifyService.NotifyOnEventClick(this, new MenuItemClickEventArgs(MenuItems.Import));
}
