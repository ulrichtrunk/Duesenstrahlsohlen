using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Models
{
    public class JqGridModel
    {
        public enum JqGridSortOrder { Asc, Desc };
        public JqGridSortOrder SortOrder { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Height { get; set; }
        public string SortName { get; set; }
        public int RowNum { get; set; } = 10;
        public List<MvcHtmlString> Columns { get; set; }

        public string Id
        {
            get
            {
                return $"{IdPrefix}{Name}";
            }
        }

        public const string IdPrefix = "jqGrid";
        public const string ContainerIdPrefix = "jqGridContainer";
        public const string PagerIdPrefix = "jqGridPager";
    }
}