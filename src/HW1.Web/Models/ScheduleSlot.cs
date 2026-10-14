namespace HW1.Web.Models;

// Just a Stub for now for the schedule
public record ScheduleSlot(
    DayOfWeek Day,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsBusy
);