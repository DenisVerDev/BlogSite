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
        private readonly ClientService service;

        [BindProperty]
        public User Client { get; set; }

        public SignInModel(BlogSiteContext db)
        {
            this.db = db;
            this.service = ClientService.GetService(TempData);
        }

        public IActionResult OnGet()
        {
            return service.IsAuthenticated ? RedirectToPage("../Index") : Page(); // client can't sign in if he is already authenticated
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if(await service.SignInAsync(ModelState, db, Client)) return RedirectToPage("../Index");
            }
            catch(Exception ex)
            {
                ViewData["ServerExceptionData"] = new ServerExceptionData(ex);
            }

            return Page();
        }
    }
}
