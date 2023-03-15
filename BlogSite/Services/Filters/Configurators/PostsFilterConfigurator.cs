using BlogSite.Models;
using BlogSite.Models.FilterModels;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogSite.Services.Filters.Configurators
{
    public class PostsFilterConfigurator
    {
        private readonly BlogSiteContext db;
        private readonly PostsFilterModel filter_model;

        public PostsFilterConfigurator(BlogSiteContext db, PostsFilterModel postsFilterModel)
        {
            this.db = db;
            this.filter_model = postsFilterModel;
        }

        public async Task ConfigureFilterAsync()
        {
            var themes = await this.GetThemesAsync();
            var periods = this.GetDatePeriods();

            filter_model.Themes.AddRange(themes);
            filter_model.DatePeriods = periods;
        }

        public async Task<List<SelectListItem>> GetThemesAsync()
        {
            ThemesService themesService = new ThemesService(db);
            var themes = await themesService.GetFormThemesAsync();

            return themes;
        }

        public List<SelectListItem> GetDatePeriods()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var values = Enum.GetValues<DatePeriod>();
            for (int i = 0; i < values.Length; i++)
            {
                string value_str = values.GetValue(i)!.ToString()!;

                string text = value_str.Replace('_', ' ').ToLower();
                text = String.Concat(text[0].ToString().ToUpper(),text.AsSpan(1));

                items.Add(new SelectListItem(text, value_str));
            }

            return items;
        }
    }
}
