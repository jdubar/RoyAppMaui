using Microsoft.AspNetCore.Components;

using MudBlazor;

namespace RoyAppMaui.Components.Controls;
public partial class TimeSelection
{
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public TimeSpan? Time { get; set; }
    [Parameter] public EventCallback<TimeSpan?> TimeChanged { get; set; }

    private MudTimePicker _timepicker = new();
}
