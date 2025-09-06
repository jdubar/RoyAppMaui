using CsvHelper;
using CsvHelper.Configuration;

namespace RoyAppMaui.Converters.Tests;
public class TimeSpanConverterTests
{
    private readonly IReaderRow _dummyReaderRow = null!;
    private readonly IWriterRow _dummyWriterRow = null!;
    private readonly MemberMapData _dummyMemberMapData = null!;

    [Theory]
    [InlineData("08:30", 8, 30, 0)]
    [InlineData("0830", 8, 30, 0)]
    [InlineData("8:30", 8, 30, 0)]
    [InlineData("8:30:00", 8, 30, 0)]
    [InlineData("08:30:00", 8, 30, 0)]
    [InlineData("8:30 AM", 8, 30, 0)]
    [InlineData("8:30 PM", 20, 30, 0)]
    public void ConvertFromString_ValidTimeSpanStrings_ReturnsTimeSpan(string input, int h, int m, int s)
    {
        // Arrange
        var converter = new TimeSpanConverter();
        var expected = new TimeSpan(h, m, s);

        // Act
        var actual = converter.ConvertFromString(input, _dummyReaderRow, _dummyMemberMapData);

        // Assert
        Assert.IsType<TimeSpan>(actual);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ConvertFromString_EmptyOrNull_ReturnsEmptyString(string? input)
    {
        // Arrange
        var converter = new TimeSpanConverter();
        var expected = string.Empty;

        // Act
        var actual = converter.ConvertFromString(input, _dummyReaderRow, _dummyMemberMapData);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ConvertFromString_InvalidFormat_ThrowsFormatException()
    {
        // Arrange
        var converter = new TimeSpanConverter();

        // Act & Assert
        Assert.Throws<FormatException>(() => converter.ConvertFromString("notatime", _dummyReaderRow, _dummyMemberMapData));
    }

    [Theory]
    [InlineData(8, 30, 0, "08:30")]
    [InlineData(0, 0, 0, "00:00")]
    public void ConvertToString_ValidTimeSpan_ReturnsFormattedString(int h, int m, int s, string expected)
    {
        // Arrange
        var converter = new TimeSpanConverter();
        var value = new TimeSpan(h, m, s);

        // Act
        var actual = converter.ConvertToString(value, _dummyWriterRow, _dummyMemberMapData);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ConvertToString_NullOrNotTimeSpan_ReturnsEmptyString()
    {
        // Arrange
        var converter = new TimeSpanConverter();

        // Act & Assert
        Assert.Equal(string.Empty, converter.ConvertToString(null, _dummyWriterRow, _dummyMemberMapData));
        Assert.Equal(string.Empty, converter.ConvertToString("notatimespan", _dummyWriterRow, _dummyMemberMapData));
    }
}