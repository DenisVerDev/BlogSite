using BlogSite.Models;
using BlogSite.Models.FilterModels;
using BlogSite.Models.PartialModels;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Services.Filters
{
    public class PostsFilter : IPostsFilter
    {
        public PostsFilterModel FilterData { get; init; }

        public int ElementsPerPage { get; init; }

        public int TotalPages { get; private set; }

        public PostsFilter(PostsFilterModel postsFilterModel, int ElementsPerPage)
        {
            this.FilterData = postsFilterModel;
            this.ElementsPerPage = ElementsPerPage;
        }

        public IQueryable<Post> FilterByTheme(IQueryable<Post> actualModel)
        {
            if (FilterData.ThemeId > 0) actualModel = actualModel.Where(x => x.Theme == FilterData.ThemeId);

            return actualModel;
        }

        public IQueryable<Post> FilterByDatePeriod(IQueryable<Post> actualModel)
        {
            if(FilterData.DatePeriod == DatePeriod.ONLY_NEWEST) actualModel = actualModel.OrderByDescending(x => x.CreationDate);
            else actualModel = actualModel.OrderByDescending(x => x.LastUpdateDate);
            
            return actualModel;
        }

        public IQueryable<Post> FilterByFavorites(IQueryable<Post> actualModel)
        {
            if (FilterData.OnlyFavorites && FilterData.Client != null) 
                actualModel = actualModel.Where(x => x.AuthorNavigation.Followers.Any(c=>c.UserId == FilterData.Client.UserId));

            return actualModel;
        }

        public IQueryable<Post> FilterByPopularity(IQueryable<Post> actualModel)
        {
            if (FilterData.MostPopular) actualModel = actualModel.OrderByDescending(x => x.Likers.LongCount());

            return actualModel;
        }

        public IQueryable<Post> FilterByPage(IQueryable<Post> actualModel)
        {
            int page_index = FilterData.Page - 1;

            return actualModel.Skip(page_index * ElementsPerPage).Take(ElementsPerPage);
        }

        public async Task<int> GetTotalPagesAsync(IQueryable<Post> actualModel)
        {
            int count = await actualModel.CountAsync();
            double total_pages = (double)count / ElementsPerPage;

            return (int)Math.Ceiling(total_pages);
        }

        public async Task<List<PartialPost>> SelectFilteredDataAsync(IQueryable<Post> actualModel)
        {
            ThemesService themesService = new ThemesService();

            var filtered = await actualModel.Select(x => new PartialPost()
            {
                PostId = x.PostId,
                Author = x.AuthorNavigation,
                Theme = themesService.ConvertToPartial(x.ThemeNavigation),
                Title = x.Title,
                Likes = x.Likers.LongCount(),
                LastUpdateDate = x.LastUpdateDate
            }).ToListAsync();

            return filtered;
        }

        public async Task<List<PartialPost>> FilterAsync(IQueryable<Post> actualModel)
        {
            actualModel = this.FilterByTheme(actualModel);
            actualModel = this.FilterByDatePeriod(actualModel);
            actualModel = this.FilterByFavorites(actualModel);
            actualModel = this.FilterByPopularity(actualModel);
            
            this.TotalPages = await this.GetTotalPagesAsync(actualModel);

            actualModel = this.FilterByPage(actualModel);

            var filtered = await this.SelectFilteredDataAsync(actualModel);

            return filtered;
        }
    }
}
