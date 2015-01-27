using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Web.Models
{
    public class GridOptions
    {
        public GridOptions()
        {
            Sort = new List<GridSortOption>();
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public IEnumerable<GridSortOption> Sort { get; set; }
        public string SortExpression
        {
            get
            {
                return !Sort.Any() ? string.Empty : string.Join(", ", Sort);
            }
        }

    }

    public class GridSortOption
    {
        public string Dir { get; set; }
        public string Field { get; set; }

        public override string ToString()
        {
            var expr = string.Empty;

            if (!string.IsNullOrWhiteSpace(Field))
                expr = Field;

            return string.Format("{0} {1}", expr, Dir ?? "ASC");
        }
    }
}