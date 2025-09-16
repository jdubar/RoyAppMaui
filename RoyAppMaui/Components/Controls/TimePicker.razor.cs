namespace RoyAppMaui.Components.Controls;
public partial class TimePicker
{
    [Parameter] public string Icon { get; set; } = Icons.Material.Filled.AccessTime;
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public TimeSpan? SleepTime { get; set; } = TimeSpan.Zero;
    [Parameter] public EventCallback<TimeSpan?> SleepTimeChanged { get; set; }

    public MudTimePicker _sleepTimePicker = default!;

    private async void HandleTimeChange(TimeSpan? newTime)
    {
        SleepTime = newTime;
        await SleepTimeChanged.InvokeAsync(newTime);
    }
}
