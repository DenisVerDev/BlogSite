using BlogSite.Models;
using BlogSite.Models.PartialModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Services
{
    public class PostService
    {
        private readonly BlogSiteContext db;

        public string ModelName { get; init; }

        public PostService(BlogSiteContext db)
        {
            this.db = db;
            this.ModelName = "Post";
        }

        public PostService(BlogSiteContext db, string ModelName)
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

        public async Task<PartialPost?> GetPartialPostAsync(int id)
        {
            ThemesService themesService = new ThemesService();

            var post = await db.Posts.Where(x => x.PostId == id)
                .Select(x => new PartialPost()
                {
                    PostId = x.PostId,
                    Author = x.AuthorNavigation,
                    Theme = themesService.ConvertToPartial(x.ThemeNavigation),
                    Title = x.Title,
                    Likes = x.Likers.LongCount(),
                    CreationDate = x.CreationDate,
                    LastUpdateDate = x.LastUpdateDate
                })
                .FirstOrDefaultAsync();

            return post;
        }
    }
}
