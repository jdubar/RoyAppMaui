using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

using RoyAppMaui.Models;

namespace RoyAppMaui.Converters.Tests;
public class TimeSpanConverterTests
{
    private readonly IReaderRow _readerRow = A.Fake<IReaderRow>();
    private readonly IWriterRow _writerRow = A.Fake<IWriterRow>();
    private readonly MemberMapData _memberMapData = CreateDummyMemberMapData();

    private static MemberMapData CreateDummyMemberMapData()
    {
        var map = new DefaultClassMap<Sleep>();
        map.Map(m => m.Bedtime);
        var memberMap = map.MemberMaps[0];
        return memberMap.Data;
    }

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
        var actual = converter.ConvertFromString(input, _readerRow, _memberMapData);

        // Assert
        Assert.IsType<TimeSpan>(actual);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ConvertFromString_EmptyOrNull_ReturnsTimespanZero(string? input)
    {
        // Arrange
        var converter = new TimeSpanConverter();
        var expected = TimeSpan.Zero;

        // Act
        var actual = converter.ConvertFromString(input, _readerRow, _memberMapData);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ConvertFromString_InvalidFormat_ThrowsFormatException()
    {
        // Arrange
        var converter = new TimeSpanConverter();
        var reader = A.Fake<IReader>();
        A.CallTo(() => reader.Configuration).Returns(new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture));
        var context = new CsvContext(reader);
        A.CallTo(() => _readerRow.Context).Returns(context);

        // Act
        object action() => converter.ConvertFromString("notatime", _readerRow, _memberMapData);

        // Assert
        var exception = Assert.Throws<TypeConverterException>(action);
        Assert.Contains("Invalid TimeSpan format", exception.Message);
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
        var actual = converter.ConvertToString(value, _writerRow, _memberMapData);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ConvertToString_NullOrNotTimeSpan_ReturnsEmptyString()
    {
        // Arrange
        var converter = new TimeSpanConverter();

        // Act & Assert
        Assert.Equal(string.Empty, converter.ConvertToString(null, _writerRow, _memberMapData));
        Assert.Equal(string.Empty, converter.ConvertToString("notatimespan", _writerRow, _memberMapData));
    }
}