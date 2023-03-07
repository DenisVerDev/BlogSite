using BlogSite.Models;
using BlogSite.Models.PartialModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Services
{
    public class ThemesService
    {
        private readonly BlogSiteContext db;

        public ThemesService()
        {

        }

        public ThemesService(BlogSiteContext db)
        {
            this.db = db;
        }

        public async Task<List<PartialTheme>> LoadThemesAsync()
        {
            var themes = await db.Themes.ToListAsync();

            var partial_themes = themes.Select(x => new PartialTheme()
            {
                ThemeId = x.ThemeId,
                ThemeName = x.Name,
                ThemeImage = this.GetImageSrc(x.ThemeImage)
            }).ToList();

            return partial_themes;
        }

        public async Task<List<SelectListItem>> GetFormThemesAsync()
        {
            var themes = await this.LoadThemesAsync();
            return themes.Select(x => new SelectListItem(x.ThemeName, x.ThemeId.ToString())).ToList();
        }

        public List<SelectListItem> GetFormThemes(IEnumerable<PartialTheme> themes)
        {
            return themes.Select(x => new SelectListItem(x.ThemeName, x.ThemeId.ToString())).ToList();
        }

        public PartialTheme ConvertToPartial(Theme theme)
        {
            PartialTheme partialTheme = new PartialTheme() 
            {
                ThemeId = theme.ThemeId,
                ThemeName = theme.Name,
                ThemeImage = this.GetImageSrc(theme.ThemeImage)
            };

            return partialTheme;
        }

        private string GetImageSrc(byte[] theme_image)
        {
            var base64 = Convert.ToBase64String(theme_image);
            return $"data:image/png;base64,{base64}";
        }
    }
}
