using RoyAppMaui.Enums;

namespace RoyAppMaui.Services;
public class NotifyStateService
{
    public event EventHandler? EventClick;

    public void NotifyOnEventClick(object sender, MenuItemClickEventArgs e) =>
        EventClick?.Invoke(sender, e);
}

public class MenuItemClickEventArgs(MenuItems menuItem) : EventArgs()
{
    public MenuItems MenuItem { get; set; } = menuItem;
}