using BlogSite.Models;
using BlogSite.Models.PartialModels;
using BlogSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Pages.Post
{
    public class IndexModel : PageModel
    {
        private readonly BlogSiteContext db;

        public PartialPost? Post { get; private set; }

        public string? PostContent { get; private set; }

        public User? Client { get; private set; }

        public IndexModel(BlogSiteContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                this.InitClient();
                await this.InitPostAsync(id);
            }
            catch(Exception ex)
            {
                this.InitNoResultModel();
                ViewData["ServerMessage"] = new ServerMessage();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostLikeAsync(int post_id)
        {
            try
            {
                this.InitClient();

                if (this.Client is null)
                    throw new Exception();

                await db.Database.ExecuteSqlAsync($"insert into LikedPosts values({this.Client.UserId},{post_id})");
            }
            catch(Exception ex)
            {
                return Partial("Icons/_LikeIcon");
            }

            return Partial("Icons/_FillLikeIcon");
        }

        public async Task<IActionResult> OnPostUndoLikeAsync(int post_id)
        {
            try
            {
                this.InitClient();

                if (this.Client is null)
                    throw new Exception();

                await db.Database.ExecuteSqlAsync($"delete from LikedPosts where Liker = {this.Client!.UserId} and LikedPost = {post_id}");
            }
            catch(Exception ex)
            {
                return Partial("Icons/_FillLikeIcon");
            }

            return Partial("Icons/_LikeIcon");
        }

        private void InitClient()
        {
            ClientService clientService = new ClientService(TempData);
            this.Client = clientService.GetDeserializedClient();
        }

        private async Task InitPostAsync(int id)
        {
            PostService postService = new PostService(db);
            this.Post = await postService.GetPartialPostAsync(id);

            if (this.Client != null && this.Post != null)
            {
                this.Post.IsLiked = await postService.GetLikeStatusAsync(this.Client.UserId, id);
                this.PostContent = await postService.GetContentAsync(id);
            }
        }

        private void InitNoResultModel()
        {
            this.Post = null;
            this.Client = null;
        }
    }
}
