using System.ComponentModel.DataAnnotations.Schema;
using Portal.Model;
using System.Data.Entity.ModelConfiguration;
using Portal.Model.Report;

namespace Portal.Data.Sql.EntityFramework.Mapping.Report
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            // Primary Key
            HasKey(t => t.CategoryID);

            // Properties
            Property(t => t.CategoryID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Category", "rpt");
            Property(t => t.CategoryID).HasColumnName("CategoryID");
            Property(t => t.ParentCategoryID).HasColumnName("ParentCategoryID");
            Property(t => t.Name).HasColumnName("Name");

            // Relationships
            HasOptional(t => t.ParentCategory)
                .WithMany(t => t.SubCategories)
                .HasForeignKey(t => t.ParentCategoryID);

            
        }
    }
}
