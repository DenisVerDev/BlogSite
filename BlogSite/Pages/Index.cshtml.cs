using BlogSite.Models;
using BlogSite.Models.FilterModels;
using BlogSite.Models.PartialModels;
using BlogSite.Services;
using BlogSite.Services.Filters;
using BlogSite.Services.Filters.Partial;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages
{
    public class IndexModel : PageModel
    {
        private const int ElementsPerPage = 9;

        private readonly BlogSiteContext db;

        public List<PartialPost> Posts { get; private set; }

        public PartialPagination Pagination { get; private set; }

        [BindProperty]
        public PostsFilterModel FilterModel { get; set; }

        public IndexModel(BlogSiteContext db)
        {
            this.db = db;
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
                this.InitFilterModel();
                await this.FilterAsync();
            }
            catch (Exception ex)
            {
                this.Posts = new List<PartialPost>();
                this.Pagination = new PartialPagination();

                ViewData["ServerMessage"] = new ServerMessage();
            };

            await this.ConfigureFilterAsync();

            return Page();
        }

        private void InitFilterModel()
        {
            ClientService clientService = new ClientService(TempData);
            var client = clientService.GetDeserializedClient();

            if (FilterModel == null) FilterModel = new PostsFilterModel() { Client = client };
            else FilterModel.Client = client;
        }

        private async Task FilterAsync()
        {
            PostsFilter filter = new PostsFilter(this.db.Posts, FilterModel, ElementsPerPage);
            filter.BuildStandartFilter();

            int total_pages = await filter.GetTotalPagesAsync();

            filter.UsePagination();

            this.Posts = await filter.FilterAsync();
            this.Pagination = new PartialPagination(total_pages, FilterModel.Page);
        }

        private async Task ConfigureFilterAsync()
        {
            PartialPostsFilter ppf = new PartialPostsFilter(db);

            try
            {
                ViewData["FilterThemes"] = await ppf.GetThemesAsync();
                ViewData["FilterDatePeriods"] = ppf.GetDatePeriods();
            }
            catch(Exception ex)
            {
                ViewData["FilterThemes"] = ppf.GetBaseThemes();
                ViewData["FilterDatePeriods"] = ppf.GetBaseDatePeriods();

                ViewData["ServerMessage"] = new ServerMessage();
            }
        }
    }
}