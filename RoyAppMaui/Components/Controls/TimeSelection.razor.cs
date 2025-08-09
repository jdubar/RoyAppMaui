namespace RoyAppMaui.Components.Controls;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class TimeSelection
{
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public TimeSpan? Time { get; set; }
    [Parameter] public EventCallback<TimeSpan?> TimeChanged { get; set; }

    private MudTimePicker _timepicker = new();
}
