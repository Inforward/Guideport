using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class KnowledgeLibraryTopic
    {
        public int KnowledgeLibraryTopicID { get; set; }
        public int SiteID { get; set; }
        public string Topic { get; set; }
        public string Subtopic { get; set; }

        [IgnoreDataMember]
        public Site Site { get; set; }
    }
}
