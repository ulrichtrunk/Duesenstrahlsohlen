using Shared;
using System;

namespace Website.Code
{
    public static class JqGrid
    {
        public static SortAndPagingParameters MapSortAndPagingParameters(this JqGridSortAndPagingParameters parameters)
        {
            return new SortAndPagingParameters()
            {
                Page = parameters.page,
                PageSize = parameters.rows,
                Sort = parameters.sidx,
                SortDir = string.Equals(parameters.sord, "asc", StringComparison.OrdinalIgnoreCase) ? SortDirection.Asc : SortDirection.Desc
            };
        }


        /// <summary>
        /// Gets the json data for grid.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static object GetJsonDataForGrid<T>(this PagedList<T> pagedList, JqGridSortAndPagingParameters parameters)
        {
            return new
            {
                total = Math.Ceiling(pagedList.Total / (decimal)parameters.rows), // Total number of pages
                page = pagedList.Page,
                records = pagedList.Records,
                rows = pagedList.Rows
            };
        }
    }
}