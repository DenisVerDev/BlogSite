namespace BlogSite.Models.PartialModels
{
    public class PartialAuthor
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int TotalFollowers { get; set; }
        public int TotalFollowing { get; set; }
        public int TotalLikes { get; set; }
        public bool IsFollowed { get; set; } = false;
    }
}
