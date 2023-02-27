using BlogSite.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Services
{
    public class PostService
    {
        private readonly BlogSiteContext db;

        public string ModelName { get; init; }

        public PostService(BlogSiteContext db, string ModelName = "Post")
        {
            this.db = db;
            this.ModelName = ModelName;
        }

        public async Task<bool> CreatePostAsync(Post post, ModelStateDictionary ModelState)
        {
            bool postexisted = await db.Posts.AnyAsync(x => x.Author == post.Author && x.Title == post.Title);

            if (!postexisted)
            {
                db.Posts.Add(post);
                await db.SaveChangesAsync();
            }
            else ModelState.AddModelError($"{ModelName}.Title", "You have already created a post with this title!");

            ModelState.Remove($"{ModelName}.AuthorNavigation"); // remove unnecessary check

            return ModelState.IsValid;
        }
    }
}
