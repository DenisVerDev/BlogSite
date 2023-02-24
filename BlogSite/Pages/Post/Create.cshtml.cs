using BlogSite.Models;
using BlogSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Pages.Post
{
    public class CreateModel : PageModel
    {
        private readonly BlogSiteContext db;
        private readonly ClientService service;

        public List<SelectListItem> Themes { get; private set; }

        [BindProperty]
        public Models.Post Post { get; set; }

        [BindProperty]
        public string PostWriterHTML { get; set; }

        public CreateModel(BlogSiteContext db)
        {
            this.db = db;
            this.service = ClientService.GetService(TempData);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if(service.IsAuthenticated)
            {
                Themes = await db.Themes.Select(x=> new SelectListItem(x.Name, x.ThemeId.ToString())).ToListAsync();
                return Page();
            }

            return RedirectToPage("../Authentication/Index"); // if not authenticated => redirect to Log In page
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage();    
        }
    }
}
