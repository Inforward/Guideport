using System.Collections.Generic;
using System.ServiceModel;
using Portal.Model;

namespace Portal.Services.Contracts
{
    public interface IAffiliateServiceChannel : IAffiliateService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface IAffiliateService
    {
        [OperationContract]
        IEnumerable<Affiliate> GetAffiliates(AffiliateRequest request);

        [OperationContract]
        IEnumerable<Feature> GetFeatures();

        [OperationContract]
        IEnumerable<AffiliateFeature> GetAffiliateFeatures(int affiliateId);

        [OperationContract]
        IEnumerable<AffiliateObjective> GetObjectives(int affiliateId);

        [OperationContract]
        void UpdateAffiliate(ref Affiliate affiliate);

        [OperationContract]
        void UpdateAffiliateFeatures(int affiliateId, IEnumerable<AffiliateFeature> features);

        [OperationContract]
        void UpdateAffiliateObjectives(int affiliateId, IEnumerable<AffiliateObjective> objectives);

        [OperationContract]
        void UpdateAffiliateLogo(AffiliateLogo logo);
    }
}
