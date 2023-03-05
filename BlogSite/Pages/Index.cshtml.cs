using BlogSite.Models;
using BlogSite.Models.FilterModels;
using BlogSite.Models.PartialModels;
using BlogSite.Services;
using BlogSite.Services.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BlogSiteContext db;

        public List<PartialPost> Posts { get; private set; }

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

                PostsFilter filter = new PostsFilter(FilterModel);
                await filter.FilterAsync(this.db.Posts);

                this.Posts = filter.FilteredData;
            }
            catch (Exception ex)
            {
                this.Posts = new List<PartialPost>();
                ViewData["ServerMessage"] = new ServerMessage();
            }

            return Page();
        }
    }
}