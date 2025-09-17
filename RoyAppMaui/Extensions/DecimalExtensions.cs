namespace RoyAppMaui.Extensions;
public static class DecimalExtensions
{
    public static string ToFormattedString(this decimal value) => value.ToString("00.00");
}
