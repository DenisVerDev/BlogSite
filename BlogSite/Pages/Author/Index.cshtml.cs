using BlogSite.Models.FilterModels;
using BlogSite.Models.PartialModels;
using BlogSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogSite.Services;
using BlogSite.Services.Filters;
using BlogSite.Services.Filters.Configurators;

namespace BlogSite.Pages.Author
{
    public class IndexModel : PageModel
    {
        private const int ElementsPerPage = 9;

        private readonly BlogSiteContext db;

        private User? Client { get; set; }

        [BindProperty]
        public PostsFilterModel FilterModel { get; set; }

        public List<PartialPost> Posts { get; private set; }

        public PartialPagination Pagination { get; private set; }

        public PartialAuthor? Author { get; private set; }

        public bool IsAuthenticated { get { return Client != null; } }

        public bool IsClientBlog { get { return Client!.UserId == Author!.AuthorId; } }

        public IndexModel(BlogSiteContext db)
        {
            this.db = db;

            this.InitModel();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            this.InitClient();
            
            if(this.IsAuthenticated) 
                return await this.BlogResult(this.Client!.UserId);

            return RedirectToPage("../Index");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            this.InitClient();

            return await this.BlogResult(id);
        }

        private void InitModel()
        {
            this.FilterModel = new PostsFilterModel();
            this.Posts = new List<PartialPost>();
            this.Pagination = new PartialPagination();
        }

        private void InitClient()
        {
            ClientService clientService = new ClientService(TempData);
            this.Client = clientService.GetDeserializedClient();
        }

        private async Task<IActionResult> BlogResult(int author_id)
        {
            try
            {
                await this.InitAuthorAsync(author_id);
                if (this.Author != null)
                {
                    await this.FilterAsync(author_id);

                    PostsFilterConfigurator configurator = new PostsFilterConfigurator(db, FilterModel);
                    await configurator.ConfigureFilterAsync();
                }
            }
            catch (Exception ex)
            {
                this.InitModel();
                ViewData["ServerMessage"] = new ServerMessage();
            }

            return Page();
        }

        private async Task InitAuthorAsync(int author_id)
        {
            AuthorService authorService = new AuthorService(this.db);
            this.Author = await authorService.GetPartialAuthorAsync(author_id);
        }

        private async Task FilterAsync(int author_id)
        {
            PostsFilter filter = new PostsFilter(this.db.Posts, FilterModel.FilterData, ElementsPerPage);
            filter.BuildStandartFilter();
            filter.FilterByAuthor(author_id);

            int total_pages = await filter.GetTotalPagesAsync();

            filter.UsePagination();

            this.Posts = await filter.FilterAsync();
            this.Pagination = new PartialPagination(total_pages, FilterModel.FilterData.Page);
        }
    }
}
