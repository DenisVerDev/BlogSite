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
                Themes = await this.GetThemes();
                return Page();
            }

            return RedirectToPage("/Authentication/Index"); // if not authenticated => redirect to Log In page
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ClientService clientService = new ClientService(TempData);
                var client = clientService.GetDeserializedClient();

                PostService postService = new PostService(db);
                this.Post.Author = client.UserId;

                if (await postService.CreatePostAsync(this.Post, ModelState)) return RedirectToPage("/Post/Index",new { id = this.Post.PostId });
            }
            catch (Exception ex)
            {
                ViewData["ServerMessage"] = new ServerMessage();
            }

            Themes = await this.GetThemes();

            return Page();
        }

        private async Task<List<SelectListItem>> GetThemes()
        {
            ThemesService themesService = new ThemesService(db);

            return await themesService.GetFormThemesAsync();
        }
    }
}
