using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HW1.Web.Pages;

public class IndexModel : PageModel
{
    // Receieves the string from the form and stores it in BusyTimes property
    [BindProperty]
    public string BusyTimes { get; set; } = string.Empty;

    // Checks if the user wants to include weekends in the calculation    
    [BindProperty]
    public bool IncludeWeekends { get; set; } 
    // Changed to public as it allows dotnet to bind the correct checkbox value to this

    // Checks if the form was submitted
    public bool WasSubmitted { get; private set; }

    // displays the page when the user first navigates to it
    public void OnGet()
    {

    }
    // Handles the POST request. No processing is done here yet
    public void OnPost()
    {
        WasSubmitted = true;
    }
}
