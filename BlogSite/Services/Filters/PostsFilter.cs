﻿using BlogSite.Models;
using BlogSite.Models.FilterModels;
using BlogSite.Models.PartialModels;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Services.Filters
{
    public class PostsFilter : IPostsFilter
    {
        public PostsFilterModel FilterData { get; set; }

        public List<PartialPost> FilteredData { get; set; }

        public int ElementsPerPage { get; init; }

        public PostsFilter()
        {
            this.FilterData = new PostsFilterModel();
            this.FilteredData = new List<PartialPost>();

            this.ElementsPerPage = 9;
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
            if (FilterData.Page > 0) actualModel = actualModel.Skip(FilterData.Page * ElementsPerPage);

            actualModel = actualModel.Take(ElementsPerPage);

            return actualModel;
        }

        public async Task LoadFilteredDataAsync(IQueryable<Post> actualModel)
        {
            this.FilteredData = await actualModel.Select(x => new PartialPost()
            {
                PostId = x.PostId,
                Author = x.AuthorNavigation,
                ThemeId = x.Theme,
                Title = x.Title,
                Likes = x.Likers.LongCount(),
                LastUpdateDate = x.LastUpdateDate
            }).ToListAsync();
        }

        public async Task FilterAsync(IQueryable<Post> actualModel)
        {
            actualModel = this.FilterByDatePeriod(actualModel);
            actualModel = this.FilterByFavorites(actualModel);
            actualModel = this.FilterByPopularity(actualModel);
            actualModel = this.FilterByPage(actualModel);

            await this.LoadFilteredDataAsync(actualModel);
        }
    }
}
