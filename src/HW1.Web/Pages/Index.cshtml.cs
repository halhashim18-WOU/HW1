using HW1.Web.Models;
using HW1.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HW1.Web.Pages;

public class IndexModel : PageModel
{
    // Receives the busy-time string entered in the form.
    [BindProperty]
    public string BusyTimes { get; set; } = string.Empty;

    // Checks whether weekends should be included in the schedule.
    [BindProperty]
    public bool IncludeWeekends { get; set; }

    // Stores the calculated schedule after the form is submitted.
    public IReadOnlyList<ScheduleSlot> Schedule { get; private set; } = [];

    // Stores an error message when the busy-time input is invalid.
    public string? ErrorMessage { get; private set; }

    // Checks whether the form was successfully submitted.
    public bool WasSubmitted { get; private set; }

    // Displays the page when the user first navigates to it.
    public void OnGet()
    {
    }

    // Parses the input and calculates the schedule after user submits the request.
    public void OnPost()
    {
        try
        {
            var busyTimeRanges = BusyTimeParser.Parse(BusyTimes);

            Schedule = ScheduleCalculator.Calculate(
                busyTimeRanges,
                IncludeWeekends
            );

            WasSubmitted = true;
        }
        catch (FormatException exception)
        {
            ErrorMessage = exception.Message;
        }
    }
}