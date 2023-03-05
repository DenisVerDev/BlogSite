namespace BlogSite.Services.Filters
{
    public interface IFilter<ActualModel,PartialModel,FilterModel>
    {
        public FilterModel FilterData { get; set; }

        public List<PartialModel> FilteredData { get; set; }

        public int ElementsPerPage { get; init; }

        public IQueryable<ActualModel> FilterByPage(IQueryable<ActualModel> actualModel);

        public Task LoadFilteredDataAsync(IQueryable<ActualModel> actualModel);

        // Base filter method. All filter and loading modules together.
        public Task FilterAsync(IQueryable<ActualModel> actualModel);
    }
}
