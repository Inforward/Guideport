using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class SiteTemplate
    {
        public SiteTemplate()
        {
            SiteContentVersions = new List<SiteContentVersion>();
        }

        public int SiteTemplateID { get; set; }
        public int SiteID { get; set; }
        public string TemplateName { get; set; }
        public string TemplateDescription { get; set; }
        public string DefaultContent { get; set; }
        public string LayoutPath { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

        [IgnoreDataMember]
        public virtual Site Site { get; set; }

        [IgnoreDataMember]
        public ICollection<SiteContentVersion> SiteContentVersions { get; set; }
    }
}
