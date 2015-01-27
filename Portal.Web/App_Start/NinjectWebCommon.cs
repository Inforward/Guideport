using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Portal.Data;
using Portal.Data.Sql.EntityFramework;
using Portal.Data.Sql.EntityFramework.Report;
using Portal.Data.Sql.EntityFramework.Rules;
using Portal.Domain.Services;
using Portal.Infrastructure.Caching;
using Portal.Infrastructure.Configuration;
using Portal.Infrastructure.Logging;
using Portal.Services.Clients;
using Portal.Services.Contracts;
using System;
using System.Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Portal.Web.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Portal.Web.NinjectWebCommon), "Stop")]

namespace Portal.Web
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static T Resolve<T>()
        {
            return bootstrapper.Kernel.Get<T>();
        }

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            if (Settings.EnableServices)
            {
                kernel.Bind<IAffiliateService>().To<AffiliateServiceClient>();
                kernel.Bind<IBusinessPlanService>().To<BusinessPlanServiceClient>();
                kernel.Bind<IConfigurationService>().To<ConfigurationServiceClient>();
                kernel.Bind<ICmsService>().To<CmsServiceClient>();
                kernel.Bind<IEmailService>().To<EmailServiceClient>();
                kernel.Bind<IFileService>().To<FileServiceClient>();
                kernel.Bind<IGeoService>().To<GeoServiceClient>();
                kernel.Bind<IGroupService>().To<GroupServiceClient>();
                kernel.Bind<ILogService>().To<LogServiceClient>();
                kernel.Bind<IPlanningService>().To<PlanningServiceClient>();
                kernel.Bind<IReportService>().To<ReportServiceClient>();
                kernel.Bind<IRuleService>().To<RuleServiceClient>();
                kernel.Bind<ISurveyService>().To<SurveyServiceClient>();
                kernel.Bind<IUserService>().To<UserServiceClient>();

                kernel.Bind<ILogger>().To<ServiceLogger>();
            }
            else
            {
                kernel.Bind<ISurveyRepository>().To<SurveyRepository>();
                kernel.Bind<IUserRepository>().To<UserRepository>();
                kernel.Bind<IBusinessPlanRepository>().To<BusinessPlanRepository>();
                kernel.Bind<ICmsRepository>().To<CmsRepository>();
                kernel.Bind<IFileRepository>().To<FileRepository>();
                kernel.Bind<IGeoRepository>().To<GeoRepository>();
                kernel.Bind<IServicesRepository>().To<ServicesRepository>();
                kernel.Bind<IRulesRepository>().To<RulesRepository>();
                kernel.Bind<IPlanningRepository>().To<PlanningRepository>();
                kernel.Bind<ILogRepository>().To<LogRepository>();
                kernel.Bind<IGroupRepository>().To<GroupRepository>();
                kernel.Bind<IReportRepository>().To<ReportRepository>();
                kernel.Bind<IAffiliateRepository>().To<AffiliateRepository>();
                kernel.Bind<IConfigurationRepository>().To<ConfigurationRepository>();

                kernel.Bind<IAffiliateService>().To<AffiliateService>();
                kernel.Bind<IBusinessPlanService>().To<BusinessPlanService>();
                kernel.Bind<IConfigurationService>().To<ConfigurationService>();
                kernel.Bind<ICmsService>().To<CmsService>();
                kernel.Bind<IEmailService>().To<EmailService>();
                kernel.Bind<IFileService>().To<FileService>();
                kernel.Bind<IGeoService>().To<GeoService>();
                kernel.Bind<IGroupService>().To<GroupService>();
                kernel.Bind<ILogService>().To<LogService>();
                kernel.Bind<IPlanningService>().To<PlanningService>();
                kernel.Bind<IReportService>().To<ReportService>();
                kernel.Bind<IRuleService>().To<RuleService>();
                kernel.Bind<ISurveyService>().To<SurveyService>();
                kernel.Bind<IUserService>().To<UserService>();

                kernel.Bind<ILogger>().To<EntityFrameworkLogger>();
            }

            // Miscellaneous
            kernel.Bind<ICacheStorage>().To<HttpContextCacheAdapter>();
        }
    }
}
