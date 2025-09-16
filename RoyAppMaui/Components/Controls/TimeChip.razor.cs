using RoyAppMaui.Extensions;

namespace RoyAppMaui.Components.Controls;
public partial class TimeChip
{
    [Parameter] public decimal Time { get; set; } = 0m;

    public string TimeDisplay => Time.ToFormattedString();
    public string Icon { get; set; } = Icons.Material.Filled.WbSunny;
    public MudBlazor.Color IconColor { get; set; } = MudBlazor.Color.Warning;

    protected override void OnParametersSet()
    {
        if (Time is >= 20m || Time is < 6m)
        {
            Icon = Icons.Material.Filled.NightsStay;
            IconColor = MudBlazor.Color.Info;
        }
    }
}
