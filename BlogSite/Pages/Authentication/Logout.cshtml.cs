using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            TempData.Remove("User");

            return RedirectToPage("Index");
        }
    }
}
