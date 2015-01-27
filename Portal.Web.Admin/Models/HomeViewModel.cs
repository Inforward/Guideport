using System.Collections.Generic;
using System.Web;
using Portal.Model;
using Portal.Model.Report;

namespace Portal.Web.Admin.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Surveys = new List<Survey>();
            Reports = new List<View>();
        }

        public User CurrentUser { get; set; }
        public IEnumerable<Survey> Surveys { get; set; }
        public IEnumerable<View> Reports { get; set; }
        public dynamic Config { get; set; }

        public HtmlString ConfigJson
        {
            get { return new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(Config)); }
        }
    }
}