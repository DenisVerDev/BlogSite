using BlogSite.Models;
using BlogSite.Models.FilterModels;
using BlogSite.Models.PartialModels;
using BlogSite.Services;
using BlogSite.Services.Filters;
using BlogSite.Services.Filters.Partial;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BlogSiteContext db;

        private const int ElementsPerPage = 9;

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
                ClientService clientService = new ClientService(TempData);
                var client = clientService.GetDeserializedClient();

                if (FilterModel == null) FilterModel = new PostsFilterModel() { Client = client };
                else FilterModel.Client = client;

                PostsFilter filter = new PostsFilter(FilterModel, ElementsPerPage);
                this.Posts = await filter.FilterAsync(this.db.Posts);

                this.Pagination = new PartialPagination(filter.TotalPages, FilterModel.Page);
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