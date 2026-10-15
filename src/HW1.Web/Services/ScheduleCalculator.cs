using HW1.Web.Models;

namespace HW1.Web.Services;

public static class ScheduleCalculator
{
    // Creates the schedule from the busy time ranges.
    public static IReadOnlyList<ScheduleSlot> Calculate(
        IReadOnlyList<BusyTimeRange> busyTimes,
        bool includeWeekends)
    {
        var days = new List<DayOfWeek>
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday
        };

        // Weekends schedule for the weekends toggle.
        if (includeWeekends)
        {
            days.Add(DayOfWeek.Saturday);
            days.Add(DayOfWeek.Sunday);
        }

        var schedule = new List<ScheduleSlot>();

        var firstTime = new TimeOnly(9, 0);
        var lastTime = new TimeOnly(17, 0);

        foreach (var day in days)
        {
            // Create one schedule slot for every hour of the day.
            for (
                var startTime = firstTime;
                startTime < lastTime;
                startTime = startTime.AddHours(1)
            )
            {
                var endTime = startTime.AddHours(1);

                // Check whether any busy range overlaps this schedule slot.
                var isBusy = busyTimes.Any(
                    busyTime =>
                        busyTime.Day == day &&
                        busyTime.StartTime < endTime &&
                        busyTime.EndTime > startTime
                );

                schedule.Add(
                    new ScheduleSlot(
                        day,
                        startTime,
                        endTime,
                        isBusy
                    )
                );
            }
        }

        return schedule;
    }
}