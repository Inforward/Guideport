using Portal.Model;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;
using System.Collections.Generic;

namespace Portal.Services.Clients
{
    public class AffiliateServiceClient : IAffiliateService
    {
        private readonly ServiceClient<IAffiliateServiceChannel> _affiliateService = new ServiceClient<IAffiliateServiceChannel>();

        public IEnumerable<Affiliate> GetAffiliates(AffiliateRequest request)
        {
            var proxy = _affiliateService.CreateProxy();
            return proxy.GetAffiliates(request);
        }

        public IEnumerable<Feature> GetFeatures()
        {
            var proxy = _affiliateService.CreateProxy();
            return proxy.GetFeatures();
        }

        public IEnumerable<AffiliateFeature> GetAffiliateFeatures(int affiliateId)
        {
            var proxy = _affiliateService.CreateProxy();
            return proxy.GetAffiliateFeatures(affiliateId);
        }

        public IEnumerable<AffiliateObjective> GetObjectives(int affiliateId)
        {
            var proxy = _affiliateService.CreateProxy();
            return proxy.GetObjectives(affiliateId);
        }

        public void UpdateAffiliate(ref Affiliate affiliate)
        {
            var proxy = _affiliateService.CreateProxy();
            proxy.UpdateAffiliate(ref affiliate);
        }

        public void UpdateAffiliateFeatures(int affiliateId, IEnumerable<AffiliateFeature> features)
        {
            var proxy = _affiliateService.CreateProxy();
            proxy.UpdateAffiliateFeatures(affiliateId, features);
        }

        public void UpdateAffiliateObjectives(int affiliateId, IEnumerable<AffiliateObjective> objectives)
        {
            var proxy = _affiliateService.CreateProxy();
            proxy.UpdateAffiliateObjectives(affiliateId, objectives);
        }

        public void UpdateAffiliateLogo(AffiliateLogo logo)
        {
            var proxy = _affiliateService.CreateProxy();
            proxy.UpdateAffiliateLogo(logo);
        }

    }
}
