using BlogSite.Models;
using BlogSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DbM = BlogSite.Models;

namespace BlogSite.Pages.Post
{
    public class IndexModel : PageModel
    {
        private readonly DbM.BlogSiteContext db;

        public DbM.Post? Post { get; private set; }

        public DbM.User? Client { get; private set; }

        public IndexModel(BlogSiteContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                ClientService clientService = new ClientService(TempData);
                this.Client = clientService.GetDeserializedClient();

                this.Post = await db.Posts.Where(x => x.PostId == id).Include(x => x.AuthorNavigation).FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                this.InitNoResultModel();
                ViewData["ServerMessage"] = new ServerMessage();
            }

            return Page();
        }

        public async Task<JsonResult> OnPostLikeAsync(int id)
        {
            try
            {
                ClientService clientService = new ClientService(TempData, this.db);
                var client = clientService.GetDeserializedClient();

                if (client is null)
                    throw new Exception();

                client.LikedPosts.Add(new DbM.Post() { PostId = id });
                await clientService.UpdateAsync(client);
            }
            catch(Exception ex)
            {
                return new JsonResult(false);
            }

            return new JsonResult(true);
        }

        public async Task<JsonResult> OnPostDislikeAsync(int id)
        {
            try
            {
                ClientService clientService = new ClientService(TempData);
                
                if (!clientService.IsAuthenticated)
                    throw new Exception();

                await db.Database.ExecuteSqlAsync($"delete from LikedPosts where LikedPost = {id}");
            }
            catch(Exception ex)
            {
                return new JsonResult(false);
            }

            return new JsonResult(true);
        }

        private void InitNoResultModel()
        {
            this.Post = null;
            this.Client = null;
        }
    }
}
