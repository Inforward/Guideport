using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Model
{
    public class Pager
    {
        public Pager()
        {
            Sort = new List<SortItem>();
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public IEnumerable<SortItem> Sort { get; set; }
        public string SortExpression
        {
            get
            {
                return !Sort.Any() ? string.Empty : string.Join(", ", Sort);
            }
        }
    }

    public class SortItem
    {
        public string Dir { get; set; }
        public string Field { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Field))
                return string.Empty;

            var dir = string.Empty;

            if (!string.IsNullOrWhiteSpace(Dir) && !Dir.Equals("asc", StringComparison.InvariantCultureIgnoreCase))
                dir = Dir.ToUpper();

            return string.Format("{0} {1}", Field, dir).Trim();
        }
    }
}