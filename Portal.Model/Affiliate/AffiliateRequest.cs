
namespace Portal.Model
{
    public class AffiliateRequest : Pager
    {
        public AffiliateRequest()
        {
            UseCache = true;
            IncludeUserCount = false;
        }

        public int AffiliateID { get; set; }
        public bool IncludeUserCount { get; set; }
        public bool UseCache { get; set; }
        public bool IncludeLogos { get; set; }
        public bool IncludeSamlConfiguration { get; set; }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", AffiliateID, IncludeUserCount, IncludeSamlConfiguration, IncludeLogos);
        }
    }
}
