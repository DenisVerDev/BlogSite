namespace BlogSite.Models.PartialModels
{
    public class PartialPagination
    {
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public bool HasPrevious
        {
            get
            {
                return CurrentPage > 1;
            }
        }

        public bool HasNext
        {
            get
            {
                return CurrentPage < TotalPages; 
            }
        }

        public PartialPagination()
        {
            this.TotalPages = 1;
            this.CurrentPage = 1;
        }

        public PartialPagination(int TotalPages, int CurrentPage)
        {
            this.TotalPages = TotalPages;
            this.CurrentPage = CurrentPage;
        }
    }
}
