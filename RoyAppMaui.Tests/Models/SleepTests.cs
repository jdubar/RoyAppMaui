using RoyAppMaui.Models;

namespace RoyAppMaui.Tests.Models;
public class SleepTests
{
    [Fact]
    public void BedtimeRec_ReturnsDecimalHours()
    {
        // Arrange
        var sleep = new Sleep { Bedtime = new TimeSpan(22, 30, 0) };

        // Act & Assert
        Assert.Equal(22.5m, sleep.BedtimeRec);
    }

    [Fact]
    public void WaketimeRec_ReturnsDecimalHours()
    {
        // Arrange
        var sleep = new Sleep { Waketime = new TimeSpan(6, 15, 0) };

        // Act & Assert
        Assert.Equal(6.25m, sleep.WaketimeRec);
    }

    [Fact]
    public void BedtimeDisplay_ReturnsFormattedString()
    {
        // Arrange
        var sleep = new Sleep { Bedtime = new TimeSpan(22, 0, 0) };

        // Act & Assert
        Assert.Equal("10:00 PM", sleep.BedtimeDisplay);
    }

    [Fact]
    public void WaketimeDisplay_ReturnsFormattedString()
    {
        // Arrange
        var sleep = new Sleep { Waketime = new TimeSpan(6, 0, 0) };

        // Act & Assert
        Assert.Equal("06:00 AM", sleep.WaketimeDisplay);
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

        // Act & Assert
        Assert.Equal(expected, sleep.Duration);
    }
}
