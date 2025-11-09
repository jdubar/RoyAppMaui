using RoyAppMaui.Extensions;
using RoyAppMaui.Models;

using System.Text;

namespace RoyAppMaui.Services.Impl.Tests;
public class DataServiceTests
{
    [Fact]
    public void GetExportData_EmptyList_ReturnsHeaderAndAverages()
    {
        // Arrange
        var service = new DataService();
        var sleeps = new List<Sleep>();

        // Act
        var result = service.GetExportData(sleeps);

        // Assert
        var actual = Encoding.UTF8.GetString(result);
        Assert.Equal("""
            Id,Bedtime,Bedtime (as decimal),Waketime,Waketime (as decimal),Duration
            Bedtime Average: 0
            Waketime Average: 0
            Duration Average: 0

            """, actual);
    }

    [Fact]
    public void GetExportData_SingleSleep_ReturnsCorrectCsv()
    {
        // Arrange
        var service = new DataService();
        var sleep = new Sleep
        {
            Id = "1",
            Bedtime = "10:00 PM".ToTimeSpan(),
            Waketime = "06:00 AM".ToTimeSpan(),
        };
        var sleeps = new List<Sleep> { sleep };

        // Act
        var result = service.GetExportData(sleeps);

        // Assert
        var actual = Encoding.UTF8.GetString(result);
        Assert.Equal("""
            Id,Bedtime,Bedtime (as decimal),Waketime,Waketime (as decimal),Duration
            1,10:00 PM,22.00,06:00 AM,06.00,08.00
            Bedtime Average: 22
            Waketime Average: 6
            Duration Average: 8

            """, actual);
    }

    [Fact]
    public void GetExportData_MultipleSleeps_ReturnsCorrectAverages()
    {
        // Arrange
        var service = new DataService();
        var sleeps = new List<Sleep>
        {
            new() { Id = "1", Bedtime = new TimeSpan(22, 0, 0), Waketime = new TimeSpan(6, 0, 0) },
            new() { Id = "2", Bedtime = new TimeSpan(23, 0, 0), Waketime = new TimeSpan(7, 0, 0) }
        };

        // Act
        var result = service.GetExportData(sleeps);

        // Assert
        var actual = Encoding.UTF8.GetString(result);
        Assert.Equal("""
            Id,Bedtime,Bedtime (as decimal),Waketime,Waketime (as decimal),Duration
            1,10:00 PM,22.00,06:00 AM,06.00,08.00
            2,11:00 PM,23.00,07:00 AM,07.00,08.00
            Bedtime Average: 22.5
            Waketime Average: 6.5
            Duration Average: 8

            """, actual);
    }
}
