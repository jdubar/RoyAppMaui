using CommunityToolkit.Maui.Storage;

using FakeItEasy;

using RoyAppMaui.Extensions;
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

        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        var service = new FileService(fileSystem, fileSaver, filePicker);

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
        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        var service = new FileService(fileSystem, fileSaver, filePicker);

        // Act
        var actual = service.GetSleepDataFromCsv(filePath);

        // Assert
        Assert.True(actual.IsFailed);
        Assert.Contains("not found", actual.Errors[0].Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetSleepDataFromCsv_EmptyFilePath_ReturnsFailure()
    {
        // Arrange
        var filePath = string.Empty;
        var fileSystem = new MockFileSystem();
        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        var service = new FileService(fileSystem, fileSaver, filePicker);

        // Act
        var actual = service.GetSleepDataFromCsv(filePath);

        // Assert
        Assert.True(actual.IsFailed);
        Assert.Contains("null or empty", actual.Errors[0].Message, StringComparison.OrdinalIgnoreCase);
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
        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        var service = new FileService(fileSystem, fileSaver, filePicker);

        // Act
        var actual = service.GetSleepDataFromCsv(filePath);

        // Assert
        Assert.True(actual.IsFailed);
        Assert.Contains("CSV parsing error", actual.Errors[0].Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetSleepDataFromCsv_UnexpectedException_ReturnsFailure()
    {
        // Arrange
        var filePath = "anyfile.csv";
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        A.CallTo(() => fileSystem.File.OpenRead(A<string>._)).Throws(new Exception("Something went wrong"));
        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        var service = new FileService(fileSystem, fileSaver, filePicker);

        // Act
        var actual = service.GetSleepDataFromCsv(filePath);

        // Assert
        Assert.True(actual.IsFailed);
        Assert.Contains("unexpected error", actual.Errors[0].Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task SaveBytesToFileAsync_SavesFileSuccessfully_ReturnsSuccess()
    {
        // Arrange
        var filePath = "savedfile.txt";
        var buffer = new byte[] { 1, 2, 3, 4, 5 };
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        A.CallTo(() => fileSaver.SaveAsync(A<string>._, A<Stream>._, A<CancellationToken>._)).Returns(new FileSaverResult(filePath, null));
        var service = new FileService(fileSystem, fileSaver, filePicker);

        // Act
        var actual = await service.SaveBytesToFileAsync(buffer, filePath);

        // Assert
        Assert.True(actual.IsSuccess);
        Assert.True(actual.Value);
    }

    [Fact]
    public async Task SaveBytesToFileAsync_SaveFails_ReturnsFailure()
    {
        // Arrange
        var exception = new Exception()
        {
            Source = "UnitTest",
        };
        var filePath = "savedfile.txt";
        var buffer = new byte[] { 1, 2, 3, 4, 5 };
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        A.CallTo(() => fileSaver.SaveAsync(A<string>._, A<Stream>._, A<CancellationToken>._)).Returns(new FileSaverResult(null, exception));
        var service = new FileService(fileSystem, fileSaver, filePicker);

        // Act
        var actual = await service.SaveBytesToFileAsync(buffer, filePath);

        // Assert
        Assert.True(actual.IsFailed);
        Assert.Contains("user canceled", actual.Errors[0].Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task SaveBytesToFileAsync_UnexpectedException_ReturnsFailure()
    {
        // Arrange
        var filePath = "savedfile.txt";
        var buffer = new byte[] { 1, 2, 3, 4, 5 };
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        A.CallTo(() => fileSaver.SaveAsync(A<string>._, A<Stream>._, A<CancellationToken>._)).Throws(new FileSaveException("Oh no, my token ring!"));
        var service = new FileService(fileSystem, fileSaver, filePicker);

        // Act
        var actual = await service.SaveBytesToFileAsync(buffer, filePath);

        // Assert
        Assert.True(actual.IsFailed);
        Assert.Contains("token ring", actual.Errors[0].Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task SelectImportFileAsync_UserSelectsFile_ReturnsFilePath()
    {
        // Arrange
        var expectedFilePath = "importedfile.csv";
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        A.CallTo(() => fileSystem.File.Exists(expectedFilePath)).Returns(true);
        A.CallTo(() => fileSystem.Path.GetExtension(expectedFilePath)).Returns(".csv");
        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        A.CallTo(() => filePicker.PickAsync(A<PickOptions>._)).Returns(new FileResult(expectedFilePath));
        var service = new FileService(fileSystem, fileSaver, filePicker);

        // Act
        var actual = await service.SelectImportFileAsync();

        // Assert
        Assert.True(actual.IsSuccess);
        Assert.Equal(expectedFilePath, actual.Value);
    }

    [Fact]
    public async Task SelectImportFileAsync_UserCancels_ReturnsFailure()
    {
        // Arrange
        FileResult? result = null;
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        A.CallTo(() => filePicker.PickAsync(A<PickOptions>._)).Returns(result);
        var service = new FileService(fileSystem, fileSaver, filePicker);

        // Act
        var actual = await service.SelectImportFileAsync();

        // Assert
        Assert.True(actual.IsFailed);
        Assert.Contains("user canceled", actual.Errors[0].Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task SelectImportFileAsync_FileDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var filePath = "nonexistentfile.csv";
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        A.CallTo(() => filePicker.PickAsync(A<PickOptions>._)).Returns(Task.FromResult<FileResult?>(new FileResult(filePath)));
        var service = new FileService(fileSystem, fileSaver, filePicker);

        // Act
        var actual = await service.SelectImportFileAsync();

        // Assert
        Assert.True(actual.IsFailed);
        Assert.Contains("does not exist", actual.Errors[0].Message, StringComparison.OrdinalIgnoreCase);
    }
}
