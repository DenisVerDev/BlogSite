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
        private User? Client { get; set; }

        [BindProperty]
        public PostsFilterModel FilterModel { get; set; }

        public PartialPostsShowcase PostsShowcase { get; private set; }

        public PartialPagination Pagination { get; private set; }

        public List<PartialAuthor> Favorites { get; private set; }

        public FavoritesModel(BlogSiteContext db)
        {
            this.db = db;

            this.InitEmptyModel();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return await this.RequestHandlerResult();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return await this.RequestHandlerResult();
        }

        private void InitClient()
        {
            ClientService clientService = new ClientService(TempData);
            this.Client = clientService.GetDeserializedClient();
        }

        private void InitEmptyModel()
        {
            this.FilterModel = new PostsFilterModel();
            this.PostsShowcase = new PartialPostsShowcase();
            this.Pagination = new PartialPagination();
            this.Favorites = new List<PartialAuthor>();
        }

        private async Task<IActionResult> RequestHandlerResult()
        {
            this.InitClient();

            if(this.Client != null) return await FavoritesResult();

            return RedirectToPage("../Authentication/Index");
        }

        private async Task<IActionResult> FavoritesResult()
        {
            try
            {
                AuthorService authorService = new AuthorService(db);
                this.Favorites = await authorService.GetFavoritesAsync(this.Client!.UserId);

                await FilterAsync();

                PostsFilterConfigurator configurator = new PostsFilterConfigurator(db, this.FilterModel);
                await configurator.ConfigureFilterAsync();
            }
            catch(Exception ex)
            {
                this.InitEmptyModel();
                ViewData["ServerMessage"] = new ServerMessage();
            }

            return Page();
        }

        private async Task FilterAsync()
        {
            PostsFilter filter = new PostsFilter(this.db.Posts, FilterModel.FilterData, PostsShowcase.ElementsPerPage);
            filter.BuildStandartFilter();
            filter.FilterByFavorites(this.Client!);

            int total_pages = await filter.GetTotalPagesAsync();

            filter.UsePagination();

            this.PostsShowcase.Posts = await filter.FilterAsync();
            this.Pagination = new PartialPagination(total_pages, FilterModel.FilterData.Page);
        }
    }
}
