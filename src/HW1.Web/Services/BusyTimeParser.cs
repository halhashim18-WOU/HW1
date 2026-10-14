using System.Globalization;
using HW1.Web.Models;

namespace HW1.Web.Services;

public static class BusyTimeParser
{
    // Converts busy-time strings into BusyTimeRanges.
    // format: Monday 9:00 AM - 11:00 AM; Wednesday 2:00 PM - 4:00 PM
    public static IReadOnlyList<BusyTimeRange> Parse(string input)
    {
        // Empty input basically means no busy times.
        if (string.IsNullOrWhiteSpace(input))
        {
            return [];
        }

        // Separate each busy-time range at the semicolons.
        var rangeTexts = input.Split(
            ';',
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries
        );

        // Store all the parsed busy-time ranges.
        var ranges = new List<BusyTimeRange>();

        foreach (var rangeText in rangeTexts)
        {
            ranges.Add(ParseRange(rangeText));
        }

        return ranges;
    }

    // Converts one busy-time string into a BusyTimeRange.
    // format: Monday 9:00 AM - 11:00 AM
    private static BusyTimeRange ParseRange(string input)
    {
        // Find the first space separating the day from the times.
        var firstSpace = input.IndexOf(' ');

        if (firstSpace <= 0)
        {
            throw new FormatException(
                "The input must begin with a full day name."
            );
        }

        // Extract the day and the remaining time-range text.
        var dayText = input[..firstSpace].Trim();
        var timeRangeText = input[(firstSpace + 1)..].Trim();

        // Separate the starting and ending times at the hyphen.
        var timeParts = timeRangeText.Split(
            '-',
            StringSplitOptions.TrimEntries
        );

        if (timeParts.Length != 2)
        {
            throw new FormatException(
                "The starting and ending times must be separated by a hyphen."
            );
        }

        // Convert the written day into the built-in DayOfWeek value.
        if (!Enum.TryParse<DayOfWeek>(
                dayText,
                ignoreCase: true,
                out var day))
        {
            throw new FormatException("The day name is invalid.");
        }

        // Convert the starting time from a 12-hour string into TimeOnly.
        if (!TimeOnly.TryParseExact(
                timeParts[0].ToUpperInvariant(),
                "h:mm tt",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var startTime))
        {
            throw new FormatException("The starting time is invalid.");
        }

        // Convert the ending time from a 12-hour string into TimeOnly.
        if (!TimeOnly.TryParseExact(
                timeParts[1].ToUpperInvariant(),
                "h:mm tt",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var endTime))
        {
            throw new FormatException("The ending time is invalid.");
        }

        // Make sure both times are on whole-hour boundaries.
        if (startTime.Minute != 0 || endTime.Minute != 0)
        {
            throw new FormatException(
                "The starting and ending times must use whole hours."
            );
        }

        // Make sure the ending time occurs after the starting time.
        if (startTime >= endTime)
        {
            throw new FormatException(
                "The ending time must occur after the starting time."
            );
        }

        // Make sure the range is inside the allowed daily schedule.
        var earliestTime = new TimeOnly(9, 0);
        var latestTime = new TimeOnly(17, 0);

        if (startTime < earliestTime || endTime > latestTime)
        {
            throw new FormatException(
                "Busy times must be between 9:00 AM and 5:00 PM."
            );
        }

        return new BusyTimeRange(day, startTime, endTime);
    }
}