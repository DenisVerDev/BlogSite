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

        public async Task<bool> FollowAsync(int author_id, bool new_follow_status)
        {
            try
            { 
                var db_client = await this.db.Users.Where(x => x.UserId == this.client.UserId)
                    .Include(x => x.Authors)
                    .FirstOrDefaultAsync();

                if (db_client != null)
                {
                    var db_author = await this.db.Users.FindAsync(author_id);
                    if (db_author != null)
                    {
                        if (new_follow_status) db_client.Authors.Add(db_author);
                        else db_client.Authors.Remove(db_author);

                        await this.db.SaveChangesAsync();

                        return new_follow_status; // returns desired result if operation is successful
                    }
                }
            }
            catch(Exception ex)
            {

            }

            return !new_follow_status; // returns the previous(reverse) follow status 
        }
    }
}
