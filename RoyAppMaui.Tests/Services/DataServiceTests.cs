using RoyAppMaui.Extensions;
using RoyAppMaui.Models;
using RoyAppMaui.Services.Impl;

using System.Text;

namespace RoyAppMaui.Tests.Services;
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
        Assert.Contains("Id,Bedtime,Bedtime (as decimal),Waketime,Waketime (as decimal)", actual);
        Assert.Contains("Bedtime Average: 0", actual);
        Assert.Contains("Waketime Average: 0", actual);
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
        Assert.Contains("Id,Bedtime,Bedtime (as decimal),Waketime,Waketime (as decimal)", actual);
        Assert.Contains("1,10:00 PM,22.00,06:00 AM,06.00", actual);
        Assert.Contains("Bedtime Average: 22", actual);
        Assert.Contains("Waketime Average: 6", actual);
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
        Assert.Contains("1,10:00 PM,22.00,06:00 AM,06.00", actual);
        Assert.Contains("2,11:00 PM,23.00,07:00 AM,07.00", actual);
        Assert.Contains("Bedtime Average: 22.5", actual);
        Assert.Contains("Waketime Average: 6.5", actual);
    }
}
