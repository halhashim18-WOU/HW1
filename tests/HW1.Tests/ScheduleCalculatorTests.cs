using HW1.Web.Models;
using HW1.Web.Services;

namespace HW1.Tests;

public class ScheduleCalculatorTests
{
    [Fact]
    public void Calculate_WeekdaySchedule_MarksBusyAndFreeSlots()
    {
        // Arrange
        var busyTimes = new List<BusyTimeRange>
        {
            new(
                DayOfWeek.Monday,
                new TimeOnly(9, 0),
                new TimeOnly(11, 0)
            ),
            new(
                DayOfWeek.Monday,
                new TimeOnly(10, 0),
                new TimeOnly(12, 0)
            )
        };

        // Act
        var schedule = ScheduleCalculator.Calculate(
            busyTimes,
            includeWeekends: false
        );

        // Assert
        Assert.Equal(40, schedule.Count);

        Assert.Contains(
            schedule,
            slot =>
                slot.Day == DayOfWeek.Monday &&
                slot.StartTime == new TimeOnly(9, 0) &&
                slot.IsBusy
        );

        Assert.Contains(
            schedule,
            slot =>
                slot.Day == DayOfWeek.Monday &&
                slot.StartTime == new TimeOnly(10, 0) &&
                slot.IsBusy
        );

        Assert.Contains(
            schedule,
            slot =>
                slot.Day == DayOfWeek.Monday &&
                slot.StartTime == new TimeOnly(11, 0) &&
                slot.IsBusy
        );

        Assert.Contains(
            schedule,
            slot =>
                slot.Day == DayOfWeek.Monday &&
                slot.StartTime == new TimeOnly(12, 0) &&
                !slot.IsBusy
        );

        Assert.Contains(
            schedule,
            slot =>
                slot.Day == DayOfWeek.Tuesday &&
                slot.StartTime == new TimeOnly(9, 0) &&
                !slot.IsBusy
        );

        Assert.DoesNotContain(
            schedule,
            slot =>
                slot.Day == DayOfWeek.Saturday ||
                slot.Day == DayOfWeek.Sunday
        );
    }
}