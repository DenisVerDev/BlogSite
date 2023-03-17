using BlogSite.Models;
using BlogSite.Models.PartialModels;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Services
{
    public class AuthorService
    {
        private readonly BlogSiteContext db;
        private readonly User? client;

        public AuthorService(BlogSiteContext db, User? client)
        {
            this.db = db;
            this.client = client;
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
                    TotalLikes = x.LikedPosts.Count,
                    IsFollowed = client != null ? x.Followers.Any(c=>c.UserId == client.UserId) : false
                }).FirstOrDefaultAsync();

            return author;
        }

        // to rework and test later
        public async Task<bool> FollowAsync(int author_id, bool follow_status)
        {
            var client = await this.db.Users.FindAsync(this.client?.UserId);
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
