namespace BlogSite.Models.PartialModels
{
    public class PartialPostsShowcase
    {
        public List<PartialPost> Posts { get; set; }

        public int ElementsPerPage { get; init; }

        public PartialPostsShowcase(int max = 9)
        {
            this.Posts = new List<PartialPost>();

            this.ElementsPerPage = max;
        }
    }
}
