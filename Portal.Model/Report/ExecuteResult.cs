using System.Collections.Generic;

namespace Portal.Model.Report
{
    public class ExecuteResult
    {
        public ExecuteResult()
        {
            Columns = new List<ColumnMetadata>();
            Rows = new List<object[]>();
        }

        public List<ColumnMetadata> Columns { get; set; }
        public List<object[]> Rows { get; set; }
        public int TotalRowCount { get; set; }
    }
}