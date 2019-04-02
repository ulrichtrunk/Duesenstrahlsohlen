namespace Shared
{
    public class SortAndPagingParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; }
        public SortDirection SortDir { get; set; }
    }
}
