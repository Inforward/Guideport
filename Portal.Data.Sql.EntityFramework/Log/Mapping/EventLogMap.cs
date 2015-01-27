using Portal.Model;
using System.Data.Entity.ModelConfiguration;

namespace Portal.Data.Sql.EntityFramework.Mapping
{
    public class EventLogMap : EntityTypeConfiguration<EventLog>
    {
        public EventLogMap()
        {
            // Primary Key
            HasKey(t => t.EventLogID);

            // Properties
            // Table & Column Mappings
            ToTable("EventLog", "app");

            Property(t => t.EventLogID).HasColumnName("EventLogID");
            Property(t => t.EventTypeID).HasColumnName("EventTypeID");
            Property(t => t.EventDate).HasColumnName("EventDate");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.Message).HasColumnName("Message");
            Property(t => t.ErrorCode).HasColumnName("ErrorCode");
            Property(t => t.ErrorText).HasColumnName("ErrorText");
            Property(t => t.ServerName).HasColumnName("ServerName");
            Property(t => t.RemoteIP).HasColumnName("RemoteIP");
            Property(t => t.BrowserType).HasColumnName("BrowserType");
            Property(t => t.ScriptName).HasColumnName("ScriptName");
            Property(t => t.RequestMethod).HasColumnName("RequestMethod");
            Property(t => t.QueryString).HasColumnName("QueryString");
            Property(t => t.PostData).HasColumnName("PostData");
            Property(t => t.Referer).HasColumnName("Referer");
            Property(t => t.StackTrace).HasColumnName("StackTrace");
            Property(t => t.Source).HasColumnName("Source");
        }
    }
}
