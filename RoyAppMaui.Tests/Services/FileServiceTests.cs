using FakeItEasy;

using RoyAppMaui.Extensions;
using RoyAppMaui.Models;
using RoyAppMaui.Services.Impl;

using System.IO.Abstractions.TestingHelpers;

namespace RoyAppMaui.Services.Tests;
public class FileServiceTests
{
    [Theory]
    [InlineData("1", "22:30", "06:30")]
    [InlineData("2", "23:00", "07:00")]
    [InlineData("3", "11:15 PM", "07:15 AM")]
    [InlineData("4", "12:00 AM", "08:00 AM")]
    [InlineData("42", "10:30 PM", "06:30 AM")]
    public void GetSleepDataFromCsv_ParsesValidCsv_ReturnsSleepRecords(string id, string bedtime, string waketime)
    {
        // Arrange
        var filePath = "test.csv";
        var fileContent = $"{id},{bedtime},{waketime}";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filePath, new MockFileData(fileContent) }
        });

        var service = new FileService(fileSystem);

        // Act
        var result = service.GetSleepDataFromCsv(filePath);

        // Assert
        Assert.True(result.IsSuccess);

        var actual = result.Value.ToList();
        Assert.Single(actual);
        Assert.Equal(id, actual[0].Id);
        Assert.Equal(bedtime.ToTimeSpan(), actual[0].Bedtime);
        Assert.Equal(waketime.ToTimeSpan(), actual[0].Waketime);
    }

    [Fact]
    public void GetSleepDataFromCsv_FileNotFound_ReturnsFailure()
    {
        // Arrange
        var filePath = "nonexistent.csv";
        var fileSystem = new MockFileSystem();
        var service = new FileService(fileSystem);

        // Act
        var result = service.GetSleepDataFromCsv(filePath);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains("not found", result.Errors[0].Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetSleepDataFromCsv_MalformedCsv_ReturnsFailure()
    {
        // Arrange
        var filePath = "nonexistent.csv";
        var fileContent = "bad,data,here";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filePath, new MockFileData(fileContent) }
        });
        var service = new FileService(fileSystem);

        // Act
        var result = service.GetSleepDataFromCsv(filePath);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains("CSV parsing error", result.Errors[0].Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetSleepDataFromCsv_UnexpectedException_ReturnsFailure()
    {
        // Arrange
        var filePath = "anyfile.csv";
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        A.CallTo(() => fileSystem.File.OpenRead(A<string>._)).Throws(new Exception("Something went wrong"));

        var service = new FileService(fileSystem);

        // Act
        var result = service.GetSleepDataFromCsv(filePath);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains("unexpected error", result.Errors[0].Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetExportData_EmptyList_ReturnsHeaderAndAverages()
    {
        // Arrange
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        var service = new FileService(fileSystem);
        var sleeps = new List<Sleep>();

        // Act
        var result = service.GetExportData(sleeps);

        // Assert
        var csv = result;
        Assert.Contains("Id,Bedtime,Bedtime (as decimal),Waketime,Waketime (as decimal)", csv);
        Assert.Contains("Bedtime Average: 0", csv);
        Assert.Contains("Waketime Average: 0", csv);
    }

    [Fact]
    public void GetExportData_SingleSleep_ReturnsCorrectCsv()
    {
        // Arrange
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        var service = new FileService(fileSystem);
        var sleep = new Sleep
        {
            Id = "1",
            Bedtime = "10:00 PM".ToTimeSpan(),
            Waketime = "06:00 AM".ToTimeSpan(),
        };
        var sleeps = new List<Sleep> { sleep };

        // Act
        var actual = service.GetExportData(sleeps);

        // Assert
        Assert.Contains("Id,Bedtime,Bedtime (as decimal),Waketime,Waketime (as decimal)", actual);
        Assert.Contains("1,10:00 PM,22,06:00 AM,6", actual);
        Assert.Contains("Bedtime Average: 22", actual);
        Assert.Contains("Waketime Average: 6", actual);
    }

    [Fact]
    public void GetExportData_MultipleSleeps_ReturnsCorrectAverages()
    {
        // Arrange
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        var service = new FileService(fileSystem);
        var sleeps = new List<Sleep>
        {
            new() { Id = "1", Bedtime = "10:00 PM".ToTimeSpan(), Waketime = "06:00 AM".ToTimeSpan() },
            new() { Id = "2", Bedtime = "11:00 PM".ToTimeSpan(), Waketime = "07:00 AM".ToTimeSpan() }
        };

        // Act
        var actual = service.GetExportData(sleeps);

        // Assert
        Assert.Contains("1,10:00 PM,22,06:00 AM,6", actual);
        Assert.Contains("2,11:00 PM,23,07:00 AM,7", actual);
        Assert.Contains("Bedtime Average: 22.5", actual);
        Assert.Contains("Waketime Average: 6.5", actual);
    }
}