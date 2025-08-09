using RoyAppMaui.Models;

namespace RoyAppMaui.Extensions.Tests;
public class SleepExtensionsTests
{
    [Theory]
    [MemberData(nameof(SleepAverageTheoryData))]
    public void GetAverage_ReturnsCorrectAverage(List<Sleep> sleeps, Func<Sleep, decimal> selector, decimal expected)
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
        var actual = sleeps.GetAverage(s => s.BedtimeRec);

        // Assert
        Assert.Equal(expected, actual);
    }

    public static TheoryData<List<Sleep>, Func<Sleep, decimal>, decimal> SleepAverageTheoryData => new()
    {
        {
            new List<Sleep>
            {
                new() { Bedtime = new TimeSpan(2, 0, 0) },
                new() { Bedtime = new TimeSpan(3, 0, 0) }
            },
            s => s.BedtimeRec,
            2.5m
        },
        {
            new List<Sleep>
            {
                new() { Waketime = new TimeSpan(6, 0, 0) },
                new() { Waketime = new TimeSpan(6, 0, 0) },
                new() { Waketime = new TimeSpan(6, 0, 0) },
                new() { Waketime = new TimeSpan(6, 0, 0) }
            },
            s => s.WaketimeRec,
            6m
        }
    };

}