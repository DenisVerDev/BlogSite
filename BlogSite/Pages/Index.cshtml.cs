using BlogSite.Models;
using BlogSite.Models.FilterModels;
using BlogSite.Models.PartialModels;
using BlogSite.Services;
using BlogSite.Services.Filters;
using BlogSite.Services.Filters.Configurators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages
{
    public class IndexModel : PageModel
    {
        private const int ElementsPerPage = 9;

        private readonly BlogSiteContext db;

        [BindProperty]
        public PostsFilterModel FilterModel { get; set; }

        public List<PartialPost> Posts { get; private set; }

        public PartialPagination Pagination { get; private set; }

        public IndexModel(BlogSiteContext db)
        {
            this.db = db;

            this.FilterModel = new PostsFilterModel(); // so, FilterData can be entered when POST
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return await FilterResult();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return await FilterResult();
        }

        private async Task<IActionResult> FilterResult()
        {
            try
            {
                await this.FilterAsync();
                await this.ConfigureFilterAsync();
            }
            catch (Exception ex)
            {
                this.Posts = new List<PartialPost>();
                this.Pagination = new PartialPagination();

                ViewData["ServerMessage"] = new ServerMessage();
            };

            return Page();
        }

        private async Task FilterAsync()
        {
            PostsFilter filter = new PostsFilter(this.db.Posts, FilterModel.FilterData, ElementsPerPage);
            filter.BuildStandartFilter();

            int total_pages = await filter.GetTotalPagesAsync();

            filter.UsePagination();

            this.Posts = await filter.FilterAsync();
            this.Pagination = new PartialPagination(total_pages, FilterModel.FilterData.Page);
        }

        private async Task ConfigureFilterAsync()
        {
            PostsFilterConfigurator pfc = new PostsFilterConfigurator(db);

            var themes = await pfc.GetThemesAsync();
            var periods = pfc.GetDatePeriods();

            FilterModel.Themes.AddRange(themes);
            FilterModel.DatePeriods = periods;
        }
    }
}