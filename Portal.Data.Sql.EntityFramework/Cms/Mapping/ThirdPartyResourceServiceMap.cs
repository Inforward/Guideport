using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class ThirdPartyResourceServiceMap : EntityTypeConfiguration<ThirdPartyResourceService>
    {
        public ThirdPartyResourceServiceMap()
        {
            // Primary Key
            HasKey(t => t.SiteThirdPartyResourceServiceID);

            // Properties
            Property(t => t.SiteThirdPartyResourceServiceID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.ServiceName)
                .HasMaxLength(256);

            // Table & Column Mappings
            ToTable("SiteThirdPartyResourceService", "cms");
            Property(t => t.SiteThirdPartyResourceServiceID).HasColumnName("SiteThirdPartyResourceServiceID");
            Property(t => t.ServiceName).HasColumnName("ServiceName");
        }
    }
}
