using RoyApp.Extensions;

namespace RoyApp.Components.Controls;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class TimeChip
{
    [Parameter] public decimal Time { get; set; } = 0m;

    public string TimeDisplay => Time.ToFormattedString();
    public string Icon { get; set; } = Icons.Material.Filled.WbSunny;
    public MudBlazor.Color IconColor { get; set; } = MudBlazor.Color.Warning;

    protected override void OnParametersSet()
    {
        Icon = Time.GetIconAsSvg(defaultValue: Icon);
        IconColor = Time.GetIconColor(defaultValue: IconColor);
    }
}
