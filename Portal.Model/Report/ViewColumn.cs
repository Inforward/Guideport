
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Portal.Model.Report
{
    public class ViewColumn
    {
        private string _dataFormat;

        private int? _width;

        public int ViewID { get; set; }
        public int ColumnID { get; set; }
        public int Ordinal { get; set; }
        public string Template { get; set; }
        public bool IsSortable { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsLocked { get; set; }

        public string DataFormat
        {
            get
            {
                return !string.IsNullOrEmpty(_dataFormat) ? _dataFormat : Column != null ? Column.DataFormat : null;
            }
            set { _dataFormat = value; }
        }

        public int? Width
        {
            get
            {
                return _width.HasValue ? _width.Value : Column != null ? Column.Width : null;
            }
            set { _width = value; }
        }

        [NotMapped]
        public string Title { get { return Column != null ? Column.Title : null; } }
        [NotMapped]
        public string DataField { get { return Column != null ? Column.DataField : null; } }

        public Column Column { get; set; }

        [IgnoreDataMember]
        public View View { get; set; }
    }
}