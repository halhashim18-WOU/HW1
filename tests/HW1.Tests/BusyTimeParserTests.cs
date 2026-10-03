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
}