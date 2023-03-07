using BlogSite.Models;
using BlogSite.Models.FilterModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogSite.Services.Filters.Partial
{
    public class PartialPostsFilter
    {
        private readonly BlogSiteContext db;

        private readonly SelectListItem base_theme;
        private readonly SelectListItem base_period;

        public PartialPostsFilter(BlogSiteContext db)
        {
            this.db = db;

            this.base_theme = new SelectListItem("All", "0"); // all themes, base value: 0
            this.base_period = new SelectListItem("All time", DatePeriod.ALL_TIME.ToString());
        }

        public async Task<List<SelectListItem>> GetThemesAsync()
        {
            ThemesService themesService = new ThemesService(db);
            var themes = await themesService.GetFormThemesAsync();

            themes.Insert(0, base_theme);

            return themes;
        }

        public List<SelectListItem> GetDatePeriods()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var values = Enum.GetValues(typeof(DatePeriod));
            for (int i = 0; i < values.Length; i++)
            {
                string value = values.GetValue(i).ToString();

                string text = value.Replace('_', ' ').ToLower();
                text = string.Concat(text[0].ToString().ToUpper(),text.AsSpan(1));

                items.Add(new SelectListItem(text, value));
            }

            return items;
        }

        public List<SelectListItem> GetBaseThemes()
        {
            return new List<SelectListItem>() { base_theme };
        }

        public List<SelectListItem> GetBaseDatePeriods()
        {
            return new List<SelectListItem>() { base_period };
        }
    }
}
