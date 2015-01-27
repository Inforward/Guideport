using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Portal.Model
{
    public class KnowledgeLibrary
    {
        public KnowledgeLibrary()
        {
            Topic = string.Empty;
            Subtopic = string.Empty;
            CreatedBy = string.Empty;
        }

        public int KnowledgeLibraryID { get; set; }
        public int SiteContentID { get; set; }
        public string Topic { get; set; }
        public string Subtopic { get; set; }
        public string CreatedBy { get; set; }

        public SiteContent SiteContent { get; set; }
    }
}
