using BlogSite.Models;
using BlogSite.Models.FilterModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogSite.Services.Filters.Configurators
{
    public class PostsFilterConfigurator
    {
        private readonly BlogSiteContext db;

        public PostsFilterConfigurator(BlogSiteContext db)
        {
            this.db = db;
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
