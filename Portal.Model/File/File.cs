using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.ServiceModel;

namespace Portal.Model
{
    [MessageContract]
    public class File : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public int FileID { get; set; }

        public byte[] Data { get; set; }

        [NotMapped]
        [MessageBodyMember(Order = 1)]
        public Stream Stream { get; set; }

        [NotMapped]
        [MessageHeader(MustUnderstand = true)]
        public long Length { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public virtual FileInfo Info { get; set; }

        public void Dispose()
        {
            if (Stream == null) return;

            Stream.Close();
            Stream = null;
        }
    }
}