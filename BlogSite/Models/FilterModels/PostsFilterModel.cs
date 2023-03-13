using BlogSite.Models.PartialModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models.FilterModels
{

    public class PostsFilterModel
    {
        [BindProperty]
        public PostsFilterData FilterData { get; set; }

        public List<SelectListItem> Themes { get; set; }

        public List<SelectListItem> DatePeriods { get; set; }

        public PostsFilterModel()
        {
            this.FilterData = new PostsFilterData();

            this.Themes = new List<SelectListItem>() { new SelectListItem("All", FilterData.ThemeId.ToString()) };
            this.DatePeriods = new List<SelectListItem>() { new SelectListItem("All time", DatePeriod.ALL_TIME.ToString())};
        }
    }

}
