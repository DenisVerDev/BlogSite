using BlogSite.Models;
using BlogSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages.Post
{
    public class CreateModel : PageModel
    {
        private readonly BlogSiteContext db;
        private readonly ClientService service;

        public CreateModel(BlogSiteContext db)
        {
            this.db = db;
            this.service = ClientService.GetService(TempData);
        }

        public IActionResult OnGet()
        {
            return service.IsAuthenticated ? Page() : RedirectToPage("../Authentication/Index"); // if not authenticated => redirect to Log In page
        }
    }
}
