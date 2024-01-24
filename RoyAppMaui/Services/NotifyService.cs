namespace RoyAppMaui.Services;
public class NotifyStateService
{
    public event EventHandler? EventClick;

    public void NotifyOnEventClick(object sender) =>
        EventClick?.Invoke(sender, EventArgs.Empty);
}