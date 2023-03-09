namespace BlogSite.Services.Filters
{
    public interface IFilter<ActualModel,PartialModel,FilterModel>
    {
        public FilterModel FilterData { get; init; }

        public int ElementsPerPage { get; init; }

        public int TotalPages { get; }

        public IQueryable<ActualModel> FilterByPage(IQueryable<ActualModel> actualModel);

        public Task<int> GetTotalPagesAsync(IQueryable<ActualModel> actualModel);

        public Task<List<PartialModel>> SelectFilteredDataAsync(IQueryable<ActualModel> actualModel);

        // Base filter method. All filter and loading modules together.
        public Task<List<PartialModel>> FilterAsync(IQueryable<ActualModel> actualModel);
    }
}
