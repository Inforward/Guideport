using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Portal.Model
{
    public partial class SiteDocumentType
    {
        public SiteDocumentType()
        {
            SiteContents = new List<SiteContent>();
        }

        public int SiteDocumentTypeID { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentTypeExtension { get; set; }
        public string MIMEType { get; set; }

        [IgnoreDataMember]
        public ICollection<SiteContent> SiteContents { get; set; }

        [NotMapped]
        public ContentDocumentType DocumentType
        {
            get { return (ContentDocumentType)SiteDocumentTypeID; }
        }
    }

    public enum ContentDocumentType
    {
        Unknown = 0,

        [MappedValue(Code = "gif", SecondCode = "image/gif")]
        GIF = 1,

        [MappedValue(Code = "jpg", SecondCode = "image/jpeg")]
        JPEG = 2,

        [MappedValue(Code = "tiff", SecondCode = "image/tiff")]
        TIFF = 3,

        [MappedValue(Code = "txt", SecondCode = "text/plain")]
        Text = 4,

        [MappedValue(Code = "HTML", SecondCode = "text/html")]
        HTML = 5,

        [MappedValue(Code = "rtf", SecondCode = "application/rtf")]
        RTF = 6,

        [MappedValue(Code = "xml", SecondCode = "text/xml")]
        XML = 7,

        [MappedValue(Code = "png", SecondCode = "image/png")]
        PNG = 8,

        [MappedValue(Code = "doc", SecondCode = "application/msword")]
        MicrosoftWord = 9,

        [MappedValue(Code = "xls", SecondCode = "application/x-excel")]
        MicrosoftExcel = 10,

        [MappedValue(Code = "ppt", SecondCode = "application/ms-powerpoint")]
        MicrosoftPowerpoint = 11,

        [MappedValue(Code = "ics", SecondCode = "text/calendar")]
        iCalendar = 12,

        [MappedValue(Code = "docx", SecondCode = "application/msword")]
        MicrosoftWordDocx = 13,

        [MappedValue(Code = "xlsx", SecondCode = "application/x-excel")]
        MicrosoftExcelXlsx = 14,

        [MappedValue(Code = "pptx", SecondCode = "application/ms-powerpoint")]
        MicrosoftPowerpointPptx = 15,

        [MappedValue(Code = "pdf", SecondCode = "application/pdf")]
        AcrobatPDF = 16,

        [MappedValue(Code = "zip", SecondCode = "application/zip")]
        Zip = 17,

        [MappedValue(Code = "pinion", SecondCode = "application/octet-stream")]
        Pinion = 18,

        [MappedValue(Code = "svg", SecondCode = "image/svg+xml")]
        SVG = 19,

        [MappedValue(Code = "mpeg", SecondCode = "video/mpeg")]
        MPEG = 20,

        [MappedValue(Code = "mp4", SecondCode = "video/mp4")]
        MP4 = 21
    }
}
