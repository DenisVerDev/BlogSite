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
        private readonly BlogSiteContext db;

        private User? Client { get; set; }

        [BindProperty]
        public PostsFilterModel FilterModel { get; set; }

        public PartialPostsShowcase PostsShowcase { get; private set; }

        public PartialPagination Pagination { get; private set; }

        public PartialAuthor? Author { get; private set; }

        public bool IsAuthenticated { get { return Client != null; } }

        public bool IsClientBlog { get { return Client!.UserId == Author!.AuthorId; } }

        public IndexModel(BlogSiteContext db)
        {
            this.db = db;

            this.InitModel();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            return await this.RequestHandlerResult(id);
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            return await this.RequestHandlerResult(id);
        }

        public async Task<IActionResult> OnPostFollowAsync(int author_id, bool new_follow_status)
        {
            try
            {
                this.InitClient();

                if (IsAuthenticated && this.Client!.UserId != author_id)
                {
                    AuthorService authorService = new AuthorService(this.db, this.Client);
                    bool result = await authorService.FollowAsync(author_id, new_follow_status);

                    return Partial("Components/_ButtonFollow", result);
                }
            }
            catch(Exception ex)
            {

            }

            return Partial("Components/_ButtonFollow", !new_follow_status); // returns the previous(reverse) follow status 
        }

        private void InitModel()
        {
            this.FilterModel = new PostsFilterModel();
            this.PostsShowcase = new PartialPostsShowcase();
            this.Pagination = new PartialPagination();
        }

        private void InitClient()
        {
            ClientService clientService = new ClientService(TempData);
            this.Client = clientService.GetDeserializedClient();
        }

        private async Task<IActionResult> RequestHandlerResult(int author_id)
        {
            this.InitClient();

            if(author_id <= 0)
            {
                if (IsAuthenticated) author_id = this.Client!.UserId;
                else return RedirectToPage("../Authentication/Index");
            }

            return await BlogResult(author_id);
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
            AuthorService authorService = new AuthorService(this.db, this.Client);
            this.Author = await authorService.GetPartialAuthorAsync(author_id);
        }

        private async Task FilterAsync(int author_id)
        {
            PostsFilter filter = new PostsFilter(this.db.Posts, FilterModel.FilterData, PostsShowcase.ElementsPerPage);
            filter.BuildStandartFilter();
            filter.FilterByAuthor(author_id);

            int total_pages = await filter.GetTotalPagesAsync();

            filter.UsePagination();

            this.PostsShowcase.Posts = await filter.FilterAsync();
            this.Pagination = new PartialPagination(total_pages, FilterModel.FilterData.Page);
        }
    }
}
