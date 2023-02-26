using BlogSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages.Authentication
{
    public class LogoutModel : PageModel
    {

        public LogoutModel()
        {
        }

        public IActionResult OnGet()
        {
            ClientService service = new ClientService(TempData);
            service.SaveClient(null);

            return RedirectToPage("Index");
        }
    }
}
