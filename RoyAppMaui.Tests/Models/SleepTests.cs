namespace RoyAppMaui.Models.Tests;
public class SleepTests
{
    [Fact]
    public void BedtimeRec_ReturnsDecimalHours()
    {
        // Arrange
        var sleep = new Sleep { Bedtime = new TimeSpan(22, 30, 0) };

        // Act
        decimal actual = sleep.BedtimeAsDecimal;

        // Assert
        Assert.Equal(22.5m, actual);
    }

    [Fact]
    public void WaketimeRec_ReturnsDecimalHours()
    {
        // Arrange
        var sleep = new Sleep { Waketime = new TimeSpan(6, 15, 0) };

        // Act
        decimal actual = sleep.WaketimeAsDecimal;

        // Assert
        Assert.Equal(6.25m, actual);
    }

    [Fact]
    public void BedtimeDisplay_ReturnsFormattedString()
    {
        // Arrange
        var sleep = new Sleep { Bedtime = new TimeSpan(22, 0, 0) };

        // Act
        string actual = sleep.BedtimeDisplay;

        // Assert
        Assert.Equal("10:00 PM", actual);
    }

    [Fact]
    public void WaketimeDisplay_ReturnsFormattedString()
    {
        // Arrange
        var sleep = new Sleep { Waketime = new TimeSpan(6, 0, 0) };

        // Act
        string actual = sleep.WaketimeDisplay;

        // Assert
        Assert.Equal("06:00 AM", actual);
    }

    [Theory]
    [InlineData(22, 0, 6, 0, 8.0)]   // Overnight sleep
    [InlineData(23, 30, 7, 0, 7.5)]  // Overnight sleep
    [InlineData(1, 0, 9, 0, 8.0)]    // Same day
    [InlineData(8, 0, 8, 0, 0.0)]    // Zero duration
    [InlineData(6, 0, 5, 0, 23.0)]   // Almost full day
    public void Duration_CalculatesCorrectly(int bedHour, int bedMin, int wakeHour, int wakeMin, decimal expected)
    {
        // Arrange
        var sleep = new Sleep
        {
            Bedtime = new TimeSpan(bedHour, bedMin, 0),
            Waketime = new TimeSpan(wakeHour, wakeMin, 0)
        };

        // Act
        decimal actual = sleep.Duration;

        // Assert
        Assert.Equal(expected, actual);
    }
}
