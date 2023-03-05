namespace BlogSite.Models.FilterModels
{
    /// <summary>
    /// ONLY_NEWEST - shows recently created post(sorst by CreationDate) in right chronological order;
    /// ALL_TIME - shows every post(sorts by LastUpdateDate);
    /// </summary>
    public enum DatePeriod
    {
        ONLY_NEWEST,
        ALL_TIME
    }

    public class PostsFilterModel
    {
        public DatePeriod DatePeriod { get; set; }

        public bool OnlyFavorites { get; set; }

        public bool MostPopular { get; set; }

        public int Page { get; set; }

        public User? Client { get; set; }

        public PostsFilterModel()
        {
            this.DatePeriod = DatePeriod.ONLY_NEWEST;
            this.OnlyFavorites = false;
            this.MostPopular = false;
            this.Page = 1;

            this.Client = null;
        }
    }

}
