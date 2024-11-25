using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SensorPoC.UI.Pages
{
    public class OverviewModel : PageModel
    {
        public string ApiUrl { get; private set; }

        public void OnGet()
        {
            ApiUrl = HttpContext.Items["ApiUrl"]?.ToString() ?? "http://localhost:8081";

        }
    }
}
