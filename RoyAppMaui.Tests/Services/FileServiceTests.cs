using CommunityToolkit.Maui.Storage;

using RoyAppMaui.Extensions;

using System.IO.Abstractions.TestingHelpers;

namespace RoyAppMaui.Services.Impl.Tests;
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

        var actual = result.Value;
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
        Assert.Equal("The file at nonexistent.csv was not found.", actual.Errors[0].Message);
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
        Assert.Equal("File path is null or empty.", actual.Errors[0].Message);
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
        Assert.Equal("""
            CSV parsing error: Invalid TimeSpan format: 'data'.
            IReader state:
               ColumnCount: 3
               CurrentIndex: 1
               HeaderRecord:

            IParser state:
               ByteCount: 0
               CharCount: 13
               Row: 1
               RawRow: 1
               Count: 3
               RawRecord:
            bad,data,here

            """, actual.Errors[0].Message);
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
        Assert.Equal("An unexpected error occurred while reading the file: Value cannot be null. (Parameter 'reader')", actual.Errors[0].Message);
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
        Assert.Equal("user canceled", actual.Errors[0].Message);
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
        Assert.Equal("File save error: Oh no, my token ring!", actual.Errors[0].Message);
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
        Assert.Equal("user canceled", actual.Errors[0].Message);
    }

    [Fact]
    public async Task SelectImportFileAsync_FileDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var filePath = "nonexistentfile.csv";
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        A.CallTo(() => filePicker.PickAsync(A<PickOptions>._)).Returns(new FileResult(filePath));
        var service = new FileService(fileSystem, fileSaver, filePicker);

        // Act
        var actual = await service.SelectImportFileAsync();

        // Assert
        Assert.True(actual.IsFailed);
        Assert.Equal("Selected file does not exist.", actual.Errors[0].Message);
    }

    [Fact]
    public async Task SelectImportFileAsync_FileIsNotCsv_ReturnsFailure()
    {
        // Arrange
        var filePath = "file.txt";
        var fileSystem = A.Fake<System.IO.Abstractions.IFileSystem>();
        A.CallTo(() => fileSystem.File.Exists(filePath)).Returns(true);
        A.CallTo(() => fileSystem.Path.GetExtension(filePath)).Returns(".txt");
        var filePicker = A.Fake<IFilePicker>();
        var fileSaver = A.Fake<IFileSaver>();
        A.CallTo(() => filePicker.PickAsync(A<PickOptions>._)).Returns(new FileResult(filePath));
        var service = new FileService(fileSystem, fileSaver, filePicker);

        // Act
        var actual = await service.SelectImportFileAsync();

        // Assert
        Assert.True(actual.IsFailed);
        Assert.Equal("Selected file is not a CSV file.", actual.Errors[0].Message);
    }
}
