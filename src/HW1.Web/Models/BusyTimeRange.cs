namespace HW1.Web.Models;

public record BusyTimeRange(
    DayOfWeek Day,
    TimeOnly StartTime,
    TimeOnly EndTime
);