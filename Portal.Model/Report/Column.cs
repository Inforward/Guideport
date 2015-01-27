using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Portal.Model.Report
{
    [MessageContract]
    public class Column
    {
        public Column()
        {
            ViewColumns = new List<ViewColumn>();
        }

        public int ColumnID { get; set; }
        public string Title { get; set; }
        public string DataField { get; set; }
        public string DataFormat { get; set; }
        public string DataTypeName { get; set; }
        public int? Width { get; set; }

        [IgnoreDataMember]
        public ICollection<ViewColumn> ViewColumns { get; set; }
    }
}