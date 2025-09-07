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
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("notatime")]
    public void ToTimeSpan_ThrowsFormatException_OnInvalid(string? time)
    {
        // Arrange & Act & Assert
        Assert.Throws<FormatException>(() => time!.ToTimeSpan());
    }
}