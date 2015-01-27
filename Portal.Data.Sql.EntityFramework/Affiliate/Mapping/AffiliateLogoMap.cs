using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class AffiliateLogoMap : EntityTypeConfiguration<AffiliateLogo>
    {
        public AffiliateLogoMap()
        {
            // Primary Key
            HasKey(t => new { t.AffiliateID, t.AffiliateLogoTypeID });

            // Properties
            Property(t => t.AffiliateID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.AffiliateLogoTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("AffiliateLogo");
            Property(t => t.AffiliateID).HasColumnName("AffiliateID");
            Property(t => t.AffiliateLogoTypeID).HasColumnName("AffiliateLogoTypeID");
            Property(t => t.FileID).HasColumnName("FileID");
            Property(t => t.CreateUserID).HasColumnName("CreateUserID");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.CreateDateUtc).HasColumnName("CreateDateUTC");
            Property(t => t.ModifyUserID).HasColumnName("ModifyUserID");
            Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            Property(t => t.ModifyDateUtc).HasColumnName("ModifyDateUTC");

            // Relationships
            HasRequired(t => t.Affiliate)
                .WithMany(t => t.Logos)
                .HasForeignKey(d => d.AffiliateID);

            HasRequired(t => t.LogoType)
                .WithMany(t => t.AffiliateLogos)
                .HasForeignKey(d => d.AffiliateLogoTypeID);

            HasRequired(t => t.FileInfo)
                .WithMany(t => t.AffiliateLogos)
                .HasForeignKey(d => d.FileID);

        }
    }
}
