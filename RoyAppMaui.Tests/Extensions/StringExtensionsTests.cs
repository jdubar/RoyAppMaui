namespace RoyAppMaui.Extensions.Tests;
public class StringExtensionsTests
{
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
    public void ToTimeSpan_ParsesVariousFormats(string input, int h, int m, int s)
    {
        // Arrange
        var expected = new TimeSpan(h, m, s);

        // Act
        var actual = input.ToTimeSpan();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToTimeSpan_ThrowsFormatException_OnInvalid()
    {
        // Arrange & Act & Assert
        Assert.Throws<FormatException>(() => "notatime".ToTimeSpan());
    }
}