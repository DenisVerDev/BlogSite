﻿namespace BlogSite.Models.PartialModels
{
    public class PartialPost
    {
        public int PostId { get; set; }
        public User Author { get; set; }
        public string Title { get; set; }
        public int? ThemeId { get; set; }
        public long Likes { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}