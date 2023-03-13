using BlogSite.Models.PartialModels;
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
        [Display(Name = "Theme")]
        public int ThemeId { get; set; }

        [Display(Name = "Date period")]
        public DatePeriod DatePeriod { get; set; }

        [Display(Name = "Most popular")]
        public bool MostPopular { get; set; }

        public int Page { get; set; }

        public PostsFilterModel()
        {
            this.ThemeId = 0;
            this.DatePeriod = DatePeriod.ALL_TIME;
            this.MostPopular = false;
            this.Page = 1;
        }
    }

}
