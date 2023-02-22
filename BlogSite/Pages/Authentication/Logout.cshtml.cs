using BlogSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        private readonly ClientService service;

        public LogoutModel()
        {
            this.service = ClientService.GetService(TempData);
        }

        public IActionResult OnGet()
        {
            this.service.Logout();

            return RedirectToPage("Index");
        }
    }
}
