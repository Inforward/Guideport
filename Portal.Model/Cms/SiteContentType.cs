using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Portal.Model
{
    public partial class SiteContentType
    {
        public SiteContentType()
        {
            SiteContents = new List<SiteContent>();
        }

        public int SiteContentTypeID { get; set; }
        public string ContentTypeName { get; set; }
        public string ContentTypeDescription { get; set; }

        [IgnoreDataMember]
        public ICollection<SiteContent> SiteContents { get; set; }

        [NotMapped]
        public ContentType ContentType
        {
            get { return (ContentType)SiteContentTypeID; }
        }
    }

    public enum ContentType
    {
        ContentPage = 1,
        StaticPage = 2,
        File = 3
    }
}
