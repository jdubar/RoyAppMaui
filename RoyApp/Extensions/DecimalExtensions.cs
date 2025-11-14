namespace RoyApp.Extensions;
public static class DecimalExtensions
{
    /// <summary>
    /// Converts the specified <see cref="decimal"/> value to a string formatted with two decimal places.
    /// </summary>
    /// <param name="value">The decimal value to format.</param>
    /// <returns>A string representation of the <paramref name="value"/> formatted as "00.00".</returns>
    public static string ToFormattedString(this decimal value) => value.ToString("00.00");

    /// <summary>
    /// Determines whether the specified time falls within the night period.
    /// </summary>
    /// <remarks>The method considers "night" to start at 8:00 PM (20.00) and end at 6:00 AM (6.00). Times are
    /// evaluated in a 24-hour format.</remarks>
    /// <param name="time">The time of day, represented as a decimal, where the integer part is the hour (0 to 23) and the fractional part
    /// represents minutes.</param>
    /// <returns><see langword="true"/> if the specified time is between 8:00 PM (inclusive) and 6:00 AM (exclusive); otherwise,
    /// <see langword="false"/>.</returns>
    public static bool IsAtNight(this decimal time) => time is >= 20m or < 6m;

    /// <summary>
    /// Returns an SVG representation of an icon based on the specified time.
    /// </summary>
    /// <param name="time">The time value used to determine the icon. The method assumes the time is in a 24-hour format.</param>
    /// <param name="defaultValue">The default SVG string to return if the time does not meet the criteria for a night icon.</param>
    /// <returns>An SVG string representing a night icon if the time is at night; otherwise, the specified <paramref
    /// name="defaultValue"/>.</returns>
    public static string GetIconAsSvg(this decimal time, string defaultValue) => time.IsAtNight() ? Icons.Material.Filled.NightsStay : defaultValue;

    /// <summary>
    /// Determines the appropriate icon color based on the time of day.
    /// </summary>
    /// <remarks>This method relies on the <c>IsAtNight</c> extension method to determine whether the given
    /// time represents nighttime.</remarks>
    /// <param name="time">The time value, represented as a <see cref="decimal"/>, used to determine whether it is night.</param>
    /// <param name="defaultValue">The default color to return if it is not night.</param>
    /// <returns><see cref="MudBlazor.Color.Info"/> if the time indicates night; otherwise, the specified <paramref
    /// name="defaultValue"/>.</returns>
    public static MudBlazor.Color GetIconColor(this decimal time, MudBlazor.Color defaultValue) => time.IsAtNight() ? MudBlazor.Color.Info : defaultValue;
}
