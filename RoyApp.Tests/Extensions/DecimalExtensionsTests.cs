namespace RoyApp.Extensions.Tests;
public class DecimalExtensionsTests
{
    public class ToFormattedString
    {
        [Theory]
        [InlineData(0.0, "00.00")]
        [InlineData(1.5, "01.50")]
        [InlineData(10.0, "10.00")]
        [InlineData(123.456, "123.46")] // rounds to 2 decimals
        [InlineData(-1.2, "-01.20")]
        public void ReturnsExpected(decimal input, string expected)
        {
            // Act
            var actual = input.ToFormattedString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class IsAtNight
    {
        [Theory]
        [InlineData(20.0, true)]
        [InlineData(23.999, true)]
        [InlineData(6.0, false)]
        [InlineData(5.99, true)]
        [InlineData(0.0, true)]
        [InlineData(19.99, false)]
        public void BehavesAsExpected(decimal time, bool expected)
        {
            // Act
            var actual = time.IsAtNight();

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class GetIconAsSvg
    {
        [Fact]
        public void ReturnsNightIcon_WhenAtNight()
        {
            // Arrange
            decimal time = 22.5m;
            var defaultValue = "DefaultIcon";

            // Act
            var actual = time.GetIconAsSvg(defaultValue);

            // Assert
            Assert.Equal(MudBlazor.Icons.Material.Filled.NightsStay, actual);
        }

        [Fact]
        public void ReturnsDefault_WhenNotAtNight()
        {
            // Arrange
            decimal time = 12.0m;
            var defaultValue = "DefaultIcon";

            // Act
            var actual = time.GetIconAsSvg(defaultValue);

            // Assert
            Assert.Equal(defaultValue, actual);
        }
    }

    public class GetIconColor
    {
        [Fact]
        public void ReturnsInfo_WhenAtNight()
        {
            // Arrange
            decimal time = 21.0m;
            const MudBlazor.Color defaultColor = MudBlazor.Color.Primary;
            const MudBlazor.Color expected = MudBlazor.Color.Info;

            // Act
            var actual = time.GetIconColor(defaultColor);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnsDefault_WhenNotAtNight()
        {
            // Arrange
            decimal time = 14.0m;
            const MudBlazor.Color expected = MudBlazor.Color.Secondary;

            // Act
            var actual = time.GetIconColor(defaultValue: expected);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}