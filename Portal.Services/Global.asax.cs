using Ninject;
using Ninject.Web.Common;
using Portal.Data;
using Portal.Data.Sql.EntityFramework;
using Portal.Data.Sql.EntityFramework.Report;
using Portal.Data.Sql.EntityFramework.Rules;
using Portal.Domain.Services;
using Portal.Infrastructure.Caching;
using Portal.Infrastructure.Logging;
using Portal.Services.Contracts;
using System;

namespace Portal.Services
{
    public class Global : NinjectHttpApplication
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);

            RegisterServices(kernel);

            return kernel;
        }

        public static T Resolve<T>()
        {
            return Bootstrapper.Kernel.Get<T>();
        }

        private void RegisterServices(IKernel kernel)
        {
            // Repositories
            kernel.Bind<IAffiliateRepository>().To<AffiliateRepository>();
            kernel.Bind<IBusinessPlanRepository>().To<BusinessPlanRepository>();
            kernel.Bind<IConfigurationRepository>().To<ConfigurationRepository>();
            kernel.Bind<ICmsRepository>().To<CmsRepository>();
            kernel.Bind<IFileRepository>().To<FileRepository>();
            kernel.Bind<IGeoRepository>().To<GeoRepository>();
            kernel.Bind<IGroupRepository>().To<GroupRepository>();
            kernel.Bind<ILogRepository>().To<LogRepository>();
            kernel.Bind<IPlanningRepository>().To<PlanningRepository>();
            kernel.Bind<IReportRepository>().To<ReportRepository>();
            kernel.Bind<IRulesRepository>().To<RulesRepository>();
            kernel.Bind<ISurveyRepository>().To<SurveyRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();

            // Service Clients
            kernel.Bind<ICmsService>().To<CmsService>();
            kernel.Bind<IGeoService>().To<GeoService>();
            kernel.Bind<ILogService>().To<LogService>();
            kernel.Bind<IRuleService>().To<RuleService>();
            kernel.Bind<ISurveyService>().To<SurveyService>();
            kernel.Bind<IUserService>().To<UserService>();

            // Miscellaneous
            //kernel.Bind<ICacheStorage>().To<AppFabricCacheAdapter>();
            kernel.Bind<ICacheStorage>().To<HttpContextCacheAdapter>();
            kernel.Bind<ILogger>().To<ServiceLogger>();
        }
    }
}