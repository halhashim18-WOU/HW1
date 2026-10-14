using HW1.Web.Services;

namespace HW1.Tests;

public class BusyTimeParserTests
{
    [Fact]
    public void Parse_ValidSingleRange_ReturnsExpectedRange()
    {
        // Arrange
        const string input = "Monday 9:00 AM - 11:00 AM";

        // Act
        var ranges = BusyTimeParser.Parse(input);

        // Assert
        var range = Assert.Single(ranges);

        Assert.Equal(DayOfWeek.Monday, range.Day);
        Assert.Equal(new TimeOnly(9, 0), range.StartTime);
        Assert.Equal(new TimeOnly(11, 0), range.EndTime);
    }

    [Fact]
    public void Parse_MultipleRanges_ReturnsAllRanges()
    {
        // Arrange
        const string input =
            "Monday 9:00 AM - 11:00 AM; Wednesday 2:00 PM - 4:00 PM";

        // Act
        var ranges = BusyTimeParser.Parse(input);

        // Assert
        Assert.Collection(
            ranges,
            first =>
            {
                Assert.Equal(DayOfWeek.Monday, first.Day);
                Assert.Equal(new TimeOnly(9, 0), first.StartTime);
                Assert.Equal(new TimeOnly(11, 0), first.EndTime);
            },
            second =>
            {
                Assert.Equal(DayOfWeek.Wednesday, second.Day);
                Assert.Equal(new TimeOnly(14, 0), second.StartTime);
                Assert.Equal(new TimeOnly(16, 0), second.EndTime);
            }
        );
    }

    [Fact]
    public void Parse_EmptyInput_ReturnsNoRanges()
    {
        // Arrange
        const string input = "";

        // Act
        var ranges = BusyTimeParser.Parse(input);

        // Assert
        Assert.Empty(ranges);
    }

    [Theory]
    [InlineData("Funday 9:00 AM - 11:00 AM")]
    [InlineData("Monday 9:00 AM 11:00 AM")]
    [InlineData("Monday 9:30 AM - 11:00 AM")]
    [InlineData("Monday 11:00 AM - 9:00 AM")]
    [InlineData("Monday 8:00 AM - 10:00 AM")]
    [InlineData("Monday 4:00 PM - 6:00 PM")]
    public void Parse_InvalidInput_ThrowsFormatException(string input)
    {
        // Act and Assert
        Assert.Throws<FormatException>(
            () => BusyTimeParser.Parse(input)
        );
    }
}