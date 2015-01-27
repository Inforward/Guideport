using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model
{
    public partial class AffiliateLogo : Auditable
    {
        public int AffiliateID { get; set; }
        public int AffiliateLogoTypeID { get; set; }
        public int FileID { get; set; }
        public AffiliateLogoType LogoType { get; set; }
        public FileInfo FileInfo { get; set; }

        [IgnoreDataMember]
        public Affiliate Affiliate { get; set; }
    }

    public enum AffiliateLogoTypes
    {
        Tile = 1,
        PdfHeader = 2,
        PdfCoverSheet = 3
    }
}
