using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class ApplicationRoleAccessMap : EntityTypeConfiguration<ApplicationRoleAccess>
    {
        public ApplicationRoleAccessMap()
        {
            // Primary Key
            HasKey(t => new { t.ApplicationRoleID, t.ApplicationAccessID });

            // Properties
            Property(t => t.ApplicationRoleID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.ApplicationAccessID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Description)
                .HasMaxLength(512);

            // Table & Column Mappings
            ToTable("ApplicationRoleAccess", "usr");
            Property(t => t.ApplicationRoleID).HasColumnName("ApplicationRoleID");
            Property(t => t.ApplicationAccessID).HasColumnName("ApplicationAccessID");
            Property(t => t.Description).HasColumnName("Description");

            // Relationships
            HasRequired(t => t.ApplicationAccess)
                .WithMany(t => t.ApplicationRoleAccesses)
                .HasForeignKey(d => d.ApplicationAccessID);

            HasRequired(t => t.ApplicationRole)
                .WithMany(t => t.ApplicationRoleAccesses)
                .HasForeignKey(d => d.ApplicationRoleID);

        }
    }
}
