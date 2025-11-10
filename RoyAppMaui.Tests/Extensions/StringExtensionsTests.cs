namespace RoyAppMaui.Extensions.Tests;
public class StringExtensionsTests
{
    [Fact]
    public void ToBytes_ReturnsCorrectByteArray()
    {
        // Arrange
        var input = "Say hello to my little friend!";
        var expected = System.Text.Encoding.UTF8.GetBytes(input);

        // Act
        var actual = input.ToBytes();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToBytes_ThrowsArgumentNullException_OnNullInput()
    {
        // Arrange
        string? input = null;

        // Act
        byte[] action() => input!.ToBytes();

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("Input string cannot be null. (Parameter 'str')", exception.Message);
    }

    [Theory]
    [InlineData("0830", 8, 30, 0)]
    [InlineData("8:30", 8, 30, 0)]
    [InlineData("08:30", 8, 30, 0)]
    [InlineData("8:30:00", 8, 30, 0)]
    [InlineData("08:30:00", 8, 30, 0)]
    [InlineData("8:30 AM", 8, 30, 0)]
    [InlineData("08:30 AM", 8, 30, 0)]
    [InlineData("8:30 PM", 20, 30, 0)]
    [InlineData("08:30 PM", 20, 30, 0)]
    public void ToTimeSpan_ParsesVariousFormats(string timeAsString, int hour, int min, int sec)
    {
        // Arrange
        var expected = new TimeSpan(hour, min, sec);

        // Act
        var actual = timeAsString.ToTimeSpan();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null, "Input string is null or empty.")]
    [InlineData("", "Input string is null or empty.")]
    [InlineData("   ", "Input string is null or empty.")]
    [InlineData("notatime", "String 'notatime' was not recognized as a valid DateTime.")]
    public void ToTimeSpan_ThrowsFormatException_OnInvalid(string? time, string expected)
    {
        // Arrange - See InlineData

        // Act
        object? action() => time!.ToTimeSpan();

        // Assert
        var exception = Assert.Throws<FormatException>(action);
        Assert.Equal(expected, exception.Message);
    }

    public class IsFileCsv
    {
        [Theory]
        [InlineData("somefile.csv", true)]
        [InlineData("somefile.Csv", true)]
        [InlineData("somefile.CSV", true)]
        [InlineData("somefile.txt", false)]
        [InlineData(null, false)]
        [InlineData("fileWithNoExt", false)]
        [InlineData("fileWith.csv.txt", false)]
        [InlineData("fileWith.txt.csv", true)]
        public void ReturnsExpected_When(string? filePath, bool expected)
        {
            // Act
            var actual = filePath.IsFileCsv();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
