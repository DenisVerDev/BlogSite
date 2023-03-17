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
        private readonly BlogSiteContext db;

        [BindProperty]
        public PostsFilterModel FilterModel { get; set; }

        public PartialPostsShowcase PostsShowcase { get; private set; }

        public PartialPagination Pagination { get; private set; }

        public IndexModel(BlogSiteContext db)
        {
            this.db = db;

            this.InitModel();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return await FilterResult();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return await FilterResult();
        }

        private void InitModel()
        {
            this.FilterModel = new PostsFilterModel();
            this.PostsShowcase = new PartialPostsShowcase();
            this.Pagination = new PartialPagination();
        }

        private async Task<IActionResult> FilterResult()
        {
            try
            {
                await this.FilterAsync();

                PostsFilterConfigurator pfc = new PostsFilterConfigurator(db, FilterModel);
                await pfc.ConfigureFilterAsync();
            }
            catch (Exception ex)
            {
                this.InitModel();
                ViewData["ServerMessage"] = new ServerMessage();
            };

            return Page();
        }

        private async Task FilterAsync()
        {
            PostsFilter filter = new PostsFilter(this.db.Posts, FilterModel.FilterData, PostsShowcase.ElementsPerPage);
            filter.BuildStandartFilter();

            int total_pages = await filter.GetTotalPagesAsync();

            filter.UsePagination();

            this.PostsShowcase.Posts = await filter.FilterAsync();
            this.Pagination = new PartialPagination(total_pages, FilterModel.FilterData.Page);
        }
    }
}