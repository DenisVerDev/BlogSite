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
                    TotalLikes = x.LikedPosts.Count
                }).FirstOrDefaultAsync();

            return author;
        }

        public async Task<bool> FollowAsync(int client_id, int author_id, bool follow_status)
        {
            var client = await this.db.Users.FindAsync(client_id);
            if(client != null)
            {
                var author = await this.db.Users.FindAsync(author_id);
                if(author != null)
                {
                    if (follow_status) client.Authors.Add(author);
                    else client.Authors.Remove(author);

                    await this.db.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }
    }
}
