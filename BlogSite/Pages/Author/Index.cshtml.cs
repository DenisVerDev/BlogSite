using BlogSite.Models.FilterModels;
using BlogSite.Models.PartialModels;
using BlogSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogSite.Services;
using BlogSite.Services.Filters;
using BlogSite.Services.Filters.Configurators;
using Microsoft.EntityFrameworkCore;

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

            this.InitEmptyModel();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            return await this.RequestHandlerResult(id);
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            return await this.RequestHandlerResult(id);
        }

        public async Task<IActionResult> OnPostFollowAsync(int author_id)
        {
            try
            {
                this.InitClient();

                if (!IsAuthenticated || this.Client!.UserId == author_id)
                    throw new Exception();

                await db.Database.ExecuteSqlAsync($"insert into FollowersAuthors values({this.Client.UserId},{author_id})");
            }
            catch(Exception ex)
            {
                return Partial("Components/_ButtonFollow", false);
            }

            return Partial("Components/_ButtonFollow", true);
        }

        public async Task<IActionResult> OnPostUnfollowAsync(int author_id)
        {
            try
            {
                this.InitClient();

                if (!IsAuthenticated || this.Client!.UserId == author_id)
                    throw new Exception();

                await db.Database.ExecuteSqlAsync($"delete from FollowersAuthors where Follower = {this.Client.UserId} and Author = {author_id}");
            }
            catch(Exception ex)
            {
                return Partial("Components/_ButtonFollow", true);
            }

            return Partial("Components/_ButtonFollow", false);
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
        }

        private async Task<IActionResult> RequestHandlerResult(int author_id)
        {
            this.InitClient();

            if(author_id <= 0)
            {
                if (IsAuthenticated) author_id = this.Client!.UserId;
                else return RedirectToPage("/Authentication/Index");
            }

            return await BlogResult(author_id);
        }

        private async Task<IActionResult> BlogResult(int author_id)
        {
            try
            {
                AuthorService authorService = new AuthorService(this.db);
                this.Author = await authorService.GetPartialAuthorAsync(author_id);

                if (this.Author != null)
                {
                    if(this.Client != null) this.Author.IsFollowed = await authorService.GetFollowStatus(this.Client!.UserId, this.Author.AuthorId);

                    await this.FilterAsync();

                    PostsFilterConfigurator configurator = new PostsFilterConfigurator(db, FilterModel);
                    await configurator.ConfigureFilterAsync();
                }
            }
            catch (Exception ex)
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
            filter.FilterByAuthor(this.Author!.AuthorId);

            int total_pages = await filter.GetTotalPagesAsync();

            filter.UsePagination();

            this.PostsShowcase.Posts = await filter.FilterAsync();
            this.Pagination = new PartialPagination(total_pages, FilterModel.FilterData.Page);
        }
    }
}
