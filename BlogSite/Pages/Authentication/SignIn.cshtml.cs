using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogSite.Models;
using BlogSite.Services;
using Newtonsoft.Json;

namespace BlogSite.Pages.Authentication
{
    public class SignInModel : PageModel
    {
        private readonly BlogSiteContext db;

        [BindProperty]
        public User Client { get; set; }

        public SignInModel(BlogSiteContext db)
        {
            this.db = db;
        }

        public IActionResult OnGet()
        {
            ClientService service = new ClientService(TempData);
            return service.IsAuthenticated ? RedirectToPage("../Index") : Page(); // client can't sign in if he is already authenticated
        }

        public async Task<IActionResult> OnPost()
        {
            ClientService service = new ClientService(TempData, this.db);

            try
            {
                if(await service.SignInAsync(Client, ModelState)) return RedirectToPage("../Index");
            }
            catch(Exception ex)
            {
                service.SaveClient(null);
                ViewData["ServerMessage"] = new ServerMessage();
            }

            return Page();
        }
    }
}
