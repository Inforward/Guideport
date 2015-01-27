using System.Data.Entity.ModelConfiguration;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Services.Mapping
{
    public class EmailMap : EntityTypeConfiguration<Email>
    {
        public EmailMap()
        {
            // Primary Key
            HasKey(t => t.EmailId);

            // Properties
            Property(t => t.From)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.To)
                .IsRequired()
                .HasMaxLength(4000);

            Property(t => t.Cc)
                .IsRequired()
                .HasMaxLength(4000);

            Property(t => t.Bcc)
                .IsRequired()
                .HasMaxLength(4000);

            Property(t => t.Subject)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.Body)
                .IsRequired();

            // Table & Column Mappings
            ToTable("EM_EmailQueue");
            Property(t => t.EmailId).HasColumnName("EmailID");
            Property(t => t.From).HasColumnName("EmailFrom");
            Property(t => t.To).HasColumnName("EmailTo");
            Property(t => t.Cc).HasColumnName("EmailCC");
            Property(t => t.Bcc).HasColumnName("EmailBCC");
            Property(t => t.Subject).HasColumnName("Subject");
            Property(t => t.Body).HasColumnName("Body");
            Property(t => t.CreateDate).HasColumnName("CreateDate");
            Property(t => t.SentDate).HasColumnName("SentDate");
            Property(t => t.Attempts).HasColumnName("Attempts");
            Property(t => t.FailDate).HasColumnName("FailDate");
            Property(t => t.DeleteDate).HasColumnName("DeleteDate");
        }
    }
}