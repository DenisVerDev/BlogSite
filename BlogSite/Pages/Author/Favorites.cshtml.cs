using BlogSite.Models;
using BlogSite.Models.FilterModels;
using BlogSite.Models.PartialModels;
using BlogSite.Services;
using BlogSite.Services.Filters;
using BlogSite.Services.Filters.Configurators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages.Author
{
    public class FavoritesModel : PageModel
    {
        private readonly BlogSiteContext db;

        [BindProperty]
        public PostsFilterModel FilterModel { get; set; }

        public PartialPostsShowcase PostsShowcase { get; private set; }

        public PartialPagination Pagination { get; private set; }

        public List<PartialAuthor> Favorites { get; private set; }

        public FavoritesModel(BlogSiteContext db)
        {
            this.db = db;

            this.InitModel();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return await this.RequestHandlerResult();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return await this.RequestHandlerResult();
        }

        private void InitModel()
        {
            this.FilterModel = new PostsFilterModel();
            this.PostsShowcase = new PartialPostsShowcase();
            this.Pagination = new PartialPagination();
            this.Favorites = new List<PartialAuthor>();
        }

        private async Task<IActionResult> RequestHandlerResult()
        {
            ClientService clientService = new ClientService(TempData);
            var client = clientService.GetDeserializedClient();

            if(client != null) return await FavoritesResult(client);

            return RedirectToPage("../Authentication/Index");
        }

        private async Task<IActionResult> FavoritesResult(User client)
        {
            try
            {
                AuthorService authorService = new AuthorService(db, client);
                this.Favorites = await authorService.GetFavoritesAsync();

                await FilterAsync(client);

                PostsFilterConfigurator configurator = new PostsFilterConfigurator(db, this.FilterModel);
                await configurator.ConfigureFilterAsync();
            }
            catch(Exception ex)
            {
                this.InitModel();
                ViewData["ServerMessage"] = new ServerMessage();
            }

            return Page();
        }

        private async Task FilterAsync(User client)
        {
            PostsFilter filter = new PostsFilter(this.db.Posts, FilterModel.FilterData, PostsShowcase.ElementsPerPage);
            filter.BuildStandartFilter();
            filter.FilterByFavorites(client);

            int total_pages = await filter.GetTotalPagesAsync();

            filter.UsePagination();

            this.PostsShowcase.Posts = await filter.FilterAsync();
            this.Pagination = new PartialPagination(total_pages, FilterModel.FilterData.Page);
        }
    }
}
