using BlogSite.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Services
{
    public class ThemesService
    {
        private readonly BlogSiteContext db;

        private List<Theme> Themes { get; set; }

        public ThemesService(BlogSiteContext db)
        {
            this.db = db;
            this.Themes = new List<Theme>();
        }

        public async Task LoadThemesAsync()
        {
            Themes = await db.Themes.ToListAsync();
        }

        public List<SelectListItem> GetFormThemes()
        {
            return Themes.Select(x => new SelectListItem(x.Name, x.ThemeId.ToString())).ToList();
        }

        public string GetImageSrc(int theme_id)
        {
            var theme = Themes.Find(x => x.ThemeId == theme_id);
            if(theme != null)
            {
                var base64 = Convert.ToBase64String(theme.ThemeImage);
                return $"data:image/png;base64,{base64}";
            }

            return String.Empty;
        }

        public string GetThemeName(int theme_id)
        {
            var theme = Themes.Find(x => x.ThemeId == theme_id);
            if (theme != null) return theme.Name;

            return String.Empty;
        }
    }
}
