using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class SiteContentVersion : Auditable
    {
        public SiteContentVersion()
        {
            Affiliates = new List<Affiliate>();
        }

        public int SiteContentVersionID { get; set; }
        public int SiteContentID { get; set; }
        public int SiteTemplateID { get; set; }
        public string VersionName { get; set; }
        public string ContentText { get; set; }
        public SiteTemplate SiteTemplate { get; set; }
        public ICollection<Affiliate> Affiliates { get; set; }

        [IgnoreDataMember]
        public virtual SiteContent SiteContent { get; set; }

        public override bool Equals(object obj)
        {
            var version = obj as SiteContentVersion;

            return version != null && version.SiteContentVersionID == SiteContentVersionID;
        }

        public override int GetHashCode()
        {
            return SiteContentVersionID;
        }
    }
}
