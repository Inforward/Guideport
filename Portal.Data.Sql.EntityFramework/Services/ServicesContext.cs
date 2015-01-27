using System.Configuration;
using System.Data.Entity;
using Portal.Data.Sql.EntityFramework.Services.Mapping;
using Portal.Infrastructure.Helpers;
using Portal.Model;

namespace Portal.Data.Sql.EntityFramework.Services
{
    public partial class ServicesContext : DbContext
    {
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

        static ServicesContext()
        {
            Database.SetInitializer<ServicesContext>(null);
        }

        public ServicesContext()
            : base(_connectionString)
        {
        }

        public DbSet<Email> Emails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EmailMap());
        }
    }
}
