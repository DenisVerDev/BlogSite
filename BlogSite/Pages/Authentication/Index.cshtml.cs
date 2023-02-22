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
        private readonly ClientService service;

        [BindProperty]
        public User Client { get; set; }

        public IndexModel(BlogSiteContext db)
        {
            this.db = db;
            this.service = ClientService.GetService(TempData);
        }

        public IActionResult OnGet()
        {
            return service.IsAuthenticated ? RedirectToPage("../Index") : Page(); // client can't log in if he is already authenticated
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if(await service.LogInAsync(ModelState, db, Client)) return RedirectToPage("../Index");
            }
            catch(Exception ex)
            {
                ViewData["ServerExceptionData"] = new ServerExceptionData(ex);
            }

            return Page();
        }
    }
}
