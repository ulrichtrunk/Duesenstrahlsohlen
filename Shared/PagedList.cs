using System.Collections.Generic;

namespace Shared
{
    public class PagedList<T>
    {
        public PagedList(List<T> rows, int page, int records, int total)
        {
            Rows = rows;
            Page = page;
            Records = records;
            Total = total;
        }

        public int Total { get; set; } // Total number of records
        public int Page { get; set; } // Current page number
        public int Records { get; set; } // amount of records per page
        public IEnumerable<T> Rows { get; set; } // records for current page
    }
}
