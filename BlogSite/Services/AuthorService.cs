using BlogSite.Models;
using BlogSite.Models.PartialModels;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Services
{
    public class AuthorService
    {
        private readonly BlogSiteContext db;

        public AuthorService(BlogSiteContext db)
        {
            this.db = db;
        }

        public async Task<PartialAuthor?> GetPartialAuthorAsync(int author_id)
        {
            var author = await this.db.Users.Where(x=>x.UserId == author_id)
                .Select(x => new PartialAuthor()
                {
                    AuthorId = x.UserId,
                    AuthorName = x.Username,
                    TotalFollowers = x.Followers.Count,
                    TotalFollowing = x.Authors.Count,
                    TotalLikes = x.Posts.SelectMany(c=>c.Likers).Count()
                }).FirstOrDefaultAsync();

            return author;
        }

        public async Task<List<PartialAuthor>> GetFavoritesAsync(int author_id)
        {
            var favorites = await this.db.Users.Where(x => x.UserId == author_id)
                .SelectMany(x => x.Authors)
                .Select(x => new PartialAuthor()
                {
                    AuthorId = x.UserId,
                    AuthorName = x.Username,
                    TotalFollowers = x.Followers.Count,
                    TotalFollowing = x.Authors.Count,
                    TotalLikes = x.Posts.SelectMany(c => c.Likers).Count(),
                    IsFollowed = true
                }).ToListAsync();

            return favorites;
        }

        public async Task<bool> GetFollowStatus(int follower_id, int author_id)
        {
            var follow_status = await db.Users.Where(x => x.UserId == follower_id)
                .SelectMany(x => x.Authors)
                .AnyAsync(x=>x.UserId == author_id);

            return follow_status;
        }
    }
}
