using BlogSite.Models;
using BlogSite.Models.PartialModels;
using BlogSite.Models.FilterModels;

namespace BlogSite.Services.Filters
{
    public interface IPostsFilter : IFilter<Post, PartialPost, PostsFilterModel>
    {
        public IQueryable<Post> FilterByDatePeriod(IQueryable<Post> actualModel);

        public IQueryable<Post> FilterByFavorites(IQueryable<Post> actualModel);

        public IQueryable<Post> FilterByPopularity(IQueryable<Post> actualModel);
    }
}
