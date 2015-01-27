
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Portal.Model.Report
{
    public class ReportResponse
    {
        private List<string> _resultStrings;
        private List<dynamic> _results;

        public View View { get; set; }
        public int TotalRowCount { get; set; }

        public List<string> SerializedResults
        {
            get { return _resultStrings; }
            set { _resultStrings = value; }
        }

        [IgnoreDataMember]
        public IEnumerable<dynamic> Results
        {
            get
            {
                if (_resultStrings == null) return new List<dynamic>();

                if (_results == null)
                {
                    var converter = new ExpandoObjectConverter();

                    _results = new List<dynamic>();

                    foreach (var s in _resultStrings)
                    {
                        _results.Add(JsonConvert.DeserializeObject<ExpandoObject>(s, converter));
                    }
                }

                return _results;
            }
            set
            {
                _resultStrings = new List<string>();

                foreach (ExpandoObject v in value)
                {
                    _resultStrings.Add(JsonConvert.SerializeObject(v));
                }
            }
        }
    }
}