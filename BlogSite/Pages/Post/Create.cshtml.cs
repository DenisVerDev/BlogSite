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

        public List<SelectListItem> Themes { get; private set; }

        [BindProperty]
        public Models.Post Post { get; set; }

        public CreateModel(BlogSiteContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ClientService service = new ClientService(TempData);
            if(service.IsAuthenticated)
            {
                Themes = await db.Themes.Select(x=> new SelectListItem(x.Name, x.ThemeId.ToString())).ToListAsync();
                return Page();
            }

            return RedirectToPage("../Authentication/Index"); // if not authenticated => redirect to Log In page
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ClientService clientService = new ClientService(TempData);
                PostService postService = new PostService(db);

                var client = clientService.GetDeserializedClient();
                this.Post.Author = client.UserId;

                if (await postService.CreatePostAsync(this.Post, ModelState)) return RedirectToPage("../Index");
            }
            catch (Exception ex)
            {
                ViewData["ServerExceptionData"] = new ServerExceptionData(ex);
            }

            Themes = await db.Themes.Select(x => new SelectListItem(x.Name, x.ThemeId.ToString())).ToListAsync();

            return Page();
        }
    }
}
