using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Infrastructure.Configuration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class UserStatusMap : EntityTypeConfiguration<UserStatus>
    {
        public UserStatusMap()
        {
            // Primary Key
            HasKey(t => t.UserStatusID);

            // Properties
            // Table & Column Mappings
            ToTable("vwUserStatus", Settings.UserProfileSchema);

            Property(t => t.UserStatusID).HasColumnName("UserStatusID");
            Property(t => t.Name).HasColumnName("UserStatusName");
        }
    }
}
