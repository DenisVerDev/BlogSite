using BlogSite.Models;
using BlogSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BlogSite.Pages.Authentication
{
    public class IndexModel : PageModel
    {
        private readonly BlogSiteContext db;

        [BindProperty]
        public User Client { get; set; }

        public IndexModel(BlogSiteContext db)
        {
            this.db = db;
        }

        public IActionResult OnGet()
        {
            ClientService service = new ClientService(TempData);
            return service.IsAuthenticated ? RedirectToPage("../Index") : Page(); // client can't log in if he is already authenticated
        }

        public async Task<IActionResult> OnPost()
        {
            ClientService service = new ClientService(TempData, this.db);

            try
            {
                if(await service.LogInAsync(Client, ModelState)) return RedirectToPage("../Index");
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
