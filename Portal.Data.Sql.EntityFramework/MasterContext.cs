using System.Configuration;
using System.Data.Entity;
using InteractivePreGeneratedViews;
using Portal.Data.Sql.EntityFramework.Geo.Mapping;
using Portal.Data.Sql.EntityFramework.Mapping;
using Portal.Data.Sql.EntityFramework.Mapping.App;
using Portal.Data.Sql.EntityFramework.Mapping.Report;
using Portal.Data.Sql.EntityFramework.Rules.Mapping;
using Portal.Data.Sql.EntityFramework.Planning.Mapping;

namespace Portal.Data.Sql.EntityFramework
{
    public partial class MasterContext : DbContext
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

        static MasterContext()
        {
            Database.SetInitializer<MasterContext>(null);

            using (var context = new MasterContext())
            {
                InteractiveViews.SetViewCacheFactory(context,
                    new SqlServerViewCacheFactory(_connectionString, "EntityFrameworkViewCache", "app"));
            }
        }

        public MasterContext()
            : base(_connectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Business Plan
            modelBuilder.Configurations.Add(new BusinessPlanMap());
            modelBuilder.Configurations.Add(new EmployeeMap());
            modelBuilder.Configurations.Add(new EmployeeRoleMap());
            modelBuilder.Configurations.Add(new ObjectiveMap());
            modelBuilder.Configurations.Add(new StrategyMap());
            modelBuilder.Configurations.Add(new TacticMap());
            modelBuilder.Configurations.Add(new SwotMap());
            modelBuilder.Configurations.Add(new AffiliateObjectiveMap());

            // CMS
            modelBuilder.Configurations.Add(new SiteMap());
            modelBuilder.Configurations.Add(new SiteContentMap());
            modelBuilder.Configurations.Add(new SiteContentVersionMap());
            modelBuilder.Configurations.Add(new SiteContentStatusMap());
            modelBuilder.Configurations.Add(new SiteContentTypeMap());
            modelBuilder.Configurations.Add(new SiteDocumentTypeMap());
            modelBuilder.Configurations.Add(new SiteTemplateMap());
            modelBuilder.Configurations.Add(new ThirdPartyResourceMap());
            modelBuilder.Configurations.Add(new ThirdPartyResourceServiceMap());
            modelBuilder.Configurations.Add(new SiteMenuIconMap());
            modelBuilder.Configurations.Add(new SiteKnowledgeLibraryMap());
            modelBuilder.Configurations.Add(new SiteKnowledgeLibraryTopicMap());

            // Rules
            modelBuilder.Configurations.Add(new RulesetMap());
            modelBuilder.Configurations.Add(new RulesetHistoryMap());

            // Survey
            modelBuilder.Configurations.Add(new SurveyMap());
            modelBuilder.Configurations.Add(new SurveyAnswerMap());
            modelBuilder.Configurations.Add(new SurveyPageMap());
            modelBuilder.Configurations.Add(new SurveyQuestionMap());
            modelBuilder.Configurations.Add(new SurveyResponseMap());
            modelBuilder.Configurations.Add(new SurveyResponseAnswerMap());
            modelBuilder.Configurations.Add(new SurveyResponseHistoryMap());
            modelBuilder.Configurations.Add(new SurveyPageScoreRangeMap());
            modelBuilder.Configurations.Add(new SurveyAnswerSuggestedContentMap());

            // User
            
            modelBuilder.Configurations.Add(new BranchMap());
            modelBuilder.Configurations.Add(new ApplicationAccessMap());
            modelBuilder.Configurations.Add(new ApplicationRoleMap());
            modelBuilder.Configurations.Add(new ApplicationRoleAccessMap());
            modelBuilder.Configurations.Add(new ApplicationRoleUserMap());
            modelBuilder.Configurations.Add(new LicenseMap());
            modelBuilder.Configurations.Add(new ProfileTypeMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserStatusMap());
            modelBuilder.Configurations.Add(new ObjectCacheMap());

            // Group
            modelBuilder.Configurations.Add(new GroupMap());
            modelBuilder.Configurations.Add(new GroupUserAccessMap());

            // Geo
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new StateProvinceMap());
            modelBuilder.Configurations.Add(new TimeZoneMap());

            // Planning
            modelBuilder.Configurations.Add(new WizardMap());
            modelBuilder.Configurations.Add(new ActionItemMap());
            modelBuilder.Configurations.Add(new PhaseMap());
            modelBuilder.Configurations.Add(new ProgressMap());
            modelBuilder.Configurations.Add(new StepMap());

            // Log
            modelBuilder.Configurations.Add(new EventLogMap());
            modelBuilder.Configurations.Add(new AuditLogMap());

            // File
            modelBuilder.Configurations.Add(new FileMap());
            modelBuilder.Configurations.Add(new FileInfoMap());

            // Report
            modelBuilder.Configurations.Add(new ColumnMap());
            modelBuilder.Configurations.Add(new FilterMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new ViewColumnMap());
            modelBuilder.Configurations.Add(new ViewMap());

            // Affiliate
            modelBuilder.Configurations.Add(new AffiliateMap());
            modelBuilder.Configurations.Add(new AffiliateLogoMap());
            modelBuilder.Configurations.Add(new AffiliateLogoTypeMap());
            modelBuilder.Configurations.Add(new FeatureMap());
            modelBuilder.Configurations.Add(new FeatureSettingMap());
            modelBuilder.Configurations.Add(new AffiliateFeatureMap());
            modelBuilder.Configurations.Add(new AffiliateFeatureSettingMap());

            // App
            modelBuilder.Configurations.Add(new ConfigurationMap());
            modelBuilder.Configurations.Add(new ConfigurationSettingMap());
            modelBuilder.Configurations.Add(new EnvironmentMap());
            modelBuilder.Configurations.Add(new SettingMap());
            modelBuilder.Configurations.Add(new ConfigurationTypeMap());
        }
    }
}
