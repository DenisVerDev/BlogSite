using BlogSite.Models;
using BlogSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogSite.Pages.Post
{
    public class EditModel : PageModel
    {
        private readonly BlogSiteContext db;

        private User? Client { get; set; }

        [BindProperty]
        public Models.Post? Post { get; set; }

        public List<SelectListItem> Themes { get; private set; }

        public bool IsOwner { get; private set; }

        public EditModel(BlogSiteContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            this.InitClient();
            if(Client is null) return RedirectToPage("Authentication/Index");

            try
            {
                this.Post = await db.Posts.FindAsync(id);

                if (this.Post != null)
                {
                    IsOwner = this.Post.Author == Client.UserId;
                    if (IsOwner) await this.InitThemesAsync();
                }
            }
            catch (Exception ex)
            {
                this.InitEmptyModel();
                ViewData["ServerMessage"] = new ServerMessage();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            this.InitClient();
            if (Client is null) return RedirectToPage("Authentication/Index");

            try
            {
                var db_post = await db.Posts.FindAsync(id);

                IsOwner = db_post.Author == Client.UserId;
                if (IsOwner)
                {
                    this.Post.PostId = id;
                    this.Post.Author = Client.UserId;

                    PostService postService = new PostService(db);
                    if (await postService.UpdatePostAsync(Post, ModelState)) return RedirectToPage("/Post/Index", new { id });

                    await this.InitThemesAsync();
                }
            }
            catch(Exception ex)
            {
                this.InitEmptyModel();
                ViewData["ServerMessage"] = new ServerMessage();
            }

            return Page();
        }

        private void InitClient()
        {
            ClientService clientService = new ClientService(TempData);
            this.Client = clientService.GetDeserializedClient();
        }

        private async Task InitThemesAsync()
        {
            ThemesService themesService = new ThemesService(db);
            Themes = await themesService.GetFormThemesAsync();
        }

        private void InitEmptyModel()
        {
            this.Post = null;
            this.Themes = null;

            this.IsOwner = false;
        }
    }
}
