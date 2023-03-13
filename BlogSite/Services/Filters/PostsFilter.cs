using BlogSite.Models;
using BlogSite.Models.FilterModels;
using BlogSite.Models.PartialModels;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Services.Filters
{
    public class PostsFilter : Filter<Post, PartialPost, PostsFilterData>
    {

        public PostsFilter(IQueryable<Post> model, PostsFilterData filter_data, int elements_per_page) :
            base(model, filter_data, elements_per_page)
        {
            
        }

        public override void BuildStandartFilter()
        {
            this.FilterByTheme();
            this.FilterByDatePeriod();
            this.FilterByPopularity();
        }

        public void FilterByTheme()
        {
            if (FilterData.ThemeId > 0) Query = Query.Where(x => x.Theme == FilterData.ThemeId);
        }

        public void FilterByDatePeriod()
        {
            if (FilterData.DatePeriod == DatePeriod.ONLY_NEWEST) Query = Query.OrderByDescending(x => x.CreationDate);
            else Query = Query.OrderByDescending(x => x.LastUpdateDate);
        }

        public void FilterByPopularity()
        {
            if (FilterData.MostPopular) Query = Query.OrderByDescending(x => x.Likers.LongCount());
        }

        // to rework and test later
        public void FilterByFavorites(User client)
        {
            if (client != null)
                Query = Query.Where(x => x.AuthorNavigation.Followers.Any(c => c.UserId == client.UserId));
        }

        public override void UsePagination()
        {
            int page_index = FilterData.Page - 1;

            Query = Query.Skip(page_index * ElementsPerPage).Take(ElementsPerPage);
        }

        public override async Task<int> GetTotalPagesAsync()
        {
            int count = await Query.CountAsync();
            double total_pages = (double)count / ElementsPerPage;

            int result = (int)Math.Ceiling(total_pages);

            return result == 0 ? 1 : result;
        }

        public override async Task<List<PartialPost>> FilterAsync()
        {
            ThemesService themesService = new ThemesService();

            var filtered = await Query.Select(x => new PartialPost()
            {
                PostId = x.PostId,
                Author = x.AuthorNavigation,
                Theme = themesService.ConvertToPartial(x.ThemeNavigation),
                Title = x.Title,
                Likes = x.Likers.LongCount(),
                CreationDate = x.CreationDate,
                LastUpdateDate = x.LastUpdateDate
            }).ToListAsync();

            return filtered;
        }
    }
}
