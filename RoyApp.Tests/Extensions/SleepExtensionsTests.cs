using RoyApp.Models;

namespace RoyApp.Extensions.Tests;
public class SleepExtensionsTests
{
    [Theory]
    [MemberData(nameof(SleepAverageTheoryData))]
    public void GetAverage_ReturnsCorrectAverage(Sleep[] sleeps, Func<Sleep, decimal> selector, decimal expected)
    {
        // Act
        var actual = sleeps.GetAverage(selector);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetAverage_ReturnsZero_WhenEmpty()
    {
        // Arrange
        var expected = 0m;
        var sleeps = new List<Sleep>();

        // Act
        var actual = sleeps.GetAverage(s => s.BedtimeAsDecimal);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetAverage_ReturnsZero_WhenSleepsIsNull()
    {
        // Arrange
        var expected = 0m;
        List<Sleep>? sleeps = null;

        // Act
        var actual = sleeps!.GetAverage(s => s.BedtimeAsDecimal);

        // Assert
        Assert.Equal(expected, actual);
    }

    public static TheoryData<Sleep[], Func<Sleep, decimal>, decimal> SleepAverageTheoryData => new()
    {
        {
            [
                new() { Bedtime = new TimeSpan(2, 0, 0) },
                new() { Bedtime = new TimeSpan(3, 0, 0) }
            ],
            s => s.BedtimeAsDecimal,
            2.5m
        },
        {
            [
                new() { Waketime = new TimeSpan(6, 0, 0) },
                new() { Waketime = new TimeSpan(6, 0, 0) },
                new() { Waketime = new TimeSpan(6, 0, 0) },
                new() { Waketime = new TimeSpan(6, 0, 0) }
            ],
            s => s.WaketimeAsDecimal,
            6m
        },
        {
            [
                new() { Waketime = new TimeSpan(2, 15, 0) },
                new() { Waketime = new TimeSpan(3, 30, 0) },
                new() { Waketime = new TimeSpan(4, 45, 0) },
                new() { Waketime = new TimeSpan(5, 50, 0) }
            ],
            s => s.WaketimeAsDecimal,
            4.08m
        }
    };

}