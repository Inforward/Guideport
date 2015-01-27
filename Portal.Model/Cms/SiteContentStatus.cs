using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class SiteContentStatus
    {
        public SiteContentStatus()
        {
            SiteContents = new List<SiteContent>();
        }

        public int SiteContentStatusID { get; set; }
        public string StatusDescription { get; set; }

        [IgnoreDataMember]
        public ICollection<SiteContent> SiteContents { get; set; }

        
    }

    public enum ContentStatus
    {
        Published = 1,
        Draft = 2,
        Removed = 3
    }
}
