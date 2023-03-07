using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Date period")]
        public DatePeriod DatePeriod { get; set; }

        [Display(Name = "Only favorites")]
        public bool OnlyFavorites { get; set; }

        [Display(Name = "Most popular")]
        public bool MostPopular { get; set; }

        public int Page { get; set; }

        public User? Client { get; set; }

        public PostsFilterModel()
        {
            this.DatePeriod = DatePeriod.ALL_TIME;
            this.OnlyFavorites = false;
            this.MostPopular = false;
            this.Page = 1;

            this.Client = null;
        }
    }

}
