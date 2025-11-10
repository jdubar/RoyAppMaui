namespace RoyAppMaui.Extensions;
public static class DecimalExtensions
{
    public static string ToFormattedString(this decimal value) => value.ToString("00.00");

    public static bool IsAtNight(this decimal time) => time is >= 20m or < 6m; // time >= 20m || time < 6m;

    public static string GetIconAsSvg(this decimal time, string defaultValue) => time.IsAtNight() ? Icons.Material.Filled.NightsStay : defaultValue;

    public static MudBlazor.Color GetIconColor(this decimal time, MudBlazor.Color defaultValue) => time.IsAtNight() ? MudBlazor.Color.Info : defaultValue;
}
