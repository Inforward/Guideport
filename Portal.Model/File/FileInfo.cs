using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Portal.Model
{
    [MessageContract]
    public class FileInfo
    {
        [MessageHeader(MustUnderstand = true)]
        public int FileID { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public string Name { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public int SizeBytes { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public string Extension { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public int CreateUserID { get; set; }

        [MessageHeader(MustUnderstand = true)]
        public DateTime CreateDate { get; set; }

        [IgnoreDataMember]
        public virtual File File { get; set; }

        [IgnoreDataMember]
        public ICollection<SiteContent> SiteContents { get; set; }

        [IgnoreDataMember]
        public ICollection<AffiliateLogo> AffiliateLogos { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public decimal? SizeKiloBytes
        {
            get
            {
                if (SizeBytes > 0)
                    return SizeBytes / 1024M;

                return 0;
            }
        }
    }
}