namespace RoyAppMaui.Extensions.Tests;

public class TimeSpanExtensionsTests
{
    [Theory]
    [InlineData(0, 0, 0, "12:00 AM")]
    [InlineData(8, 30, 0, "08:30 AM")]
    [InlineData(15, 45, 0, "03:45 PM")]
    [InlineData(23, 59, 0, "11:59 PM")]
    [InlineData(24, 0, 0, "12:00 AM")] // Normalizes to next day
    [InlineData(25, 15, 0, "01:15 AM")] // Normalizes to next day
    [InlineData(-1, 0, 0, "Invalid Time")] // Negative TimeSpan
    public void ToTimeAsString_ReturnsExpectedFormat(int h, int m, int s, string expected)
    {
        // Arrange
        var ts = new TimeSpan(h, m, s);

        // Act
        var actual = ts.ToTimeAsString();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0, 0, 0.00)]
    [InlineData(1, 30, 0, 1.50)]
    [InlineData(2, 15, 0, 2.25)]
    [InlineData(10, 0, 0, 10.00)]
    [InlineData(23, 59, 59, 23.9997)] // Will be rounded to 24.00
    public void ToHoursAsDecimal_ReturnsRoundedDecimal(int h, int m, int s, decimal expected)
    {
        // Arrange
        var ts = new TimeSpan(h, m, s);
        expected = Math.Round(expected, 2);

        // Act
        var actual = ts.ToHoursAsDecimal();

        // Assert
        Assert.Equal(expected, actual);
    }
}