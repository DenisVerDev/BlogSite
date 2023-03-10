namespace BlogSite.Services.Filters
{
    public abstract class Filter<ActualModel,PartialModel,FilterModel>
    {
        protected IQueryable<ActualModel> Query { get; set; }

        public FilterModel FilterData { get; init; }

        public int ElementsPerPage { get; init; }

        public Filter(IQueryable<ActualModel> model, FilterModel filter_model, int elements_per_page)
        {
            this.Query = model;
            this.FilterData = filter_model;
            this.ElementsPerPage = elements_per_page;
        }

        public abstract void BuildStandartFilter();

        public abstract void UsePagination();

        public abstract Task<int> GetTotalPagesAsync();

        public abstract Task<List<PartialModel>> FilterAsync();
    }
}
