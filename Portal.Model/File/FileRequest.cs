using System.ServiceModel;

namespace Portal.Model
{
    [MessageContract]
    public class FileRequest
    {
        [MessageBodyMember]
        public int FileID { get; set; }
        [MessageBodyMember]
        public string ContentLocation { get; set; }
        [MessageBodyMember]
        public string Subdirectory { get; set; }
        [MessageBodyMember]
        public string FileName { get; set; }
    }
}