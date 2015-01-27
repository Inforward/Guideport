using Portal.Data;
using Portal.Infrastructure.Caching;
using Portal.Model;
using Portal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Portal.Domain.Services
{
    public class AffiliateService : BaseService, IAffiliateService
    {
        private readonly IAffiliateRepository _affiliateRepository;
        private readonly ICacheStorage _cacheStorage;
        private const string CacheKeyBase = "AffiliateService.Affiliates";

        public AffiliateService(IAffiliateRepository affiliateRepository, ICacheStorage cacheStorage)
        {
            _affiliateRepository = affiliateRepository;
            _cacheStorage = cacheStorage;
        }

        public IEnumerable<Affiliate> GetAffiliates(AffiliateRequest request)
        {
            request = request ?? new AffiliateRequest();

            var cacheKey = string.Format("{0}.{1}", CacheKeyBase, request.ToString());

            return request.UseCache ? _cacheStorage.Retrieve(cacheKey, () => GetAffiliatesInternal(request)) : GetAffiliatesInternal(request);
        }

        public IEnumerable<Feature> GetFeatures()
        {
            return _affiliateRepository.GetAll<Feature>()
                        .Include(f => f.Settings)
                        .OrderBy(f => f.Name).ToList();
        }

        public IEnumerable<AffiliateFeature> GetAffiliateFeatures(int affiliateId)
        {
            var request = new AffiliateRequest
            {
                AffiliateID = affiliateId,
                UseCache = false,
                IncludeLogos = true
            };

            var affiliate = GetAffiliates(request).FirstOrDefault();

            if(affiliate == null)
                throw new Exception("Invalid Affiliate ID");

            // Get all available features
            var features = GetFeatures().ToList();

            // Add any missing features
            foreach (var feature in features.Where(feature => affiliate.Features.All(f => f.FeatureID != feature.FeatureID)))
            {
                affiliate.Features.Add(new AffiliateFeature()
                {
                    AffiliateID = affiliate.AffiliateID,
                    FeatureID = feature.FeatureID,
                    Feature = feature,
                    IsEnabled = false
                });
            }

            // Now add any missing feature settings
            foreach (var affiliateFeature in affiliate.Features)
            {
                var feature = features.First(f => f.FeatureID == affiliateFeature.FeatureID);

                foreach (var setting in feature.Settings.Where(setting => affiliateFeature.Settings.All(s => s.FeatureSettingID != setting.FeatureSettingID)))
                {
                    affiliateFeature.Settings.Add(new AffiliateFeatureSetting()
                    {
                        FeatureSettingID = setting.FeatureSettingID,
                        AffiliateFeatureID = affiliateFeature.AffiliateFeatureID
                    });
                }
            }

            return affiliate.Features.OrderBy(f => f.Feature.Name).ToList();
        }

        public IEnumerable<AffiliateObjective> GetObjectives(int affiliateId)
        {
            var affiliateObjectives =  _affiliateRepository.FindBy<AffiliateObjective>(oa => oa.AffiliateID == affiliateId)
                                                           .Include("Objective")
                                                           .ToList();

            affiliateObjectives.ForEach(ao => ao.ObjectiveName = ao.Objective.Name);

            return affiliateObjectives;
        }

        public void UpdateAffiliate(ref Affiliate affiliate)
        {
            var incomingAffiliate = affiliate;

            var existingAffiliate = _affiliateRepository.FindBy<Affiliate>(a => a.AffiliateID == incomingAffiliate.AffiliateID).FirstOrDefault();

            if (existingAffiliate == null)
                throw new Exception("Invalid Affiliate ID");

            affiliate = _affiliateRepository.SaveGraph(incomingAffiliate);

            _cacheStorage.ClearNamespace(CacheKeyBase);
        }

        public void UpdateAffiliateFeatures(int affiliateId, IEnumerable<AffiliateFeature> features)
        {
            var affiliate = GetAffiliates(new AffiliateRequest { AffiliateID = affiliateId, UseCache = false }).FirstOrDefault();

            if(affiliate == null)
                throw new Exception("Invalid Affiliate ID");

            foreach (var feature in features)
            {
                _affiliateRepository.SaveAffiliateFeature(feature);
            }

            _cacheStorage.ClearNamespace(CacheKeyBase);
        }

        public void UpdateAffiliateObjectives(int affiliateId, IEnumerable<AffiliateObjective> objectives)
        {
            var affiliate = GetAffiliates(new AffiliateRequest { AffiliateID = affiliateId, UseCache = false }).FirstOrDefault();

            if (affiliate == null)
                throw new Exception("Invalid Affiliate ID");

            foreach (var objective in objectives)
            {
                _affiliateRepository.SaveGraph(objective);
            }

            _cacheStorage.ClearNamespace(CacheKeyBase);
        }

        public void UpdateAffiliateLogo(AffiliateLogo logo)
        {
            _affiliateRepository.SaveGraph(logo);
            _cacheStorage.ClearNamespace(CacheKeyBase);
        }

        #region Private Methods

        private IEnumerable<Affiliate> GetAffiliatesInternal(AffiliateRequest request)
        {
            var query = _affiliateRepository.GetAll<Affiliate>()
                                            .AsNoTracking()
                                            .Include("Features")
                                            .Include("Features.Settings")
                                            .Include("Features.Feature")
                                            .Include("Features.Feature.Settings");

            if (request.AffiliateID > 0)
                query = query.Where(a => a.AffiliateID == request.AffiliateID);

            if (request.IncludeLogos)
                query = query.Include("Logos")
                             .Include("Logos.LogoType")
                             .Include("Logos.FileInfo");

            if (request.IncludeSamlConfiguration)
                query = query.Include("SamlConfiguration")
                             .Include("SamlConfiguration.ConfigurationType")
                             .Include("SamlConfiguration.ConfigurationType.Settings")
                             .Include("SamlConfiguration.ConfigurationSettings")
                             .Include("SamlConfiguration.ConfigurationSettings.Environment")
                             .Include("SamlConfiguration.ConfigurationSettings.Setting");

            var affiliates = request.IncludeUserCount ? GetAffiliateUserCounts(query).ToList() : query.OrderBy(a => a.Name).ToList();

            if (request.IncludeLogos)
            {
                var logoTypes = _affiliateRepository.GetAll<AffiliateLogoType>().ToList();

                foreach (var affiliate in affiliates)
                {
                    foreach (var logoType in logoTypes.Where(logoType => !affiliate.Logos.Any(l => l.AffiliateLogoTypeID == logoType.AffiliateLogoTypeID)))
                    {
                        affiliate.Logos.Add(new AffiliateLogo()
                        {
                            AffiliateID = affiliate.AffiliateID,
                            AffiliateLogoTypeID = logoType.AffiliateLogoTypeID,
                            LogoType = logoType
                        });
                    }
                }
            }

            return affiliates;
        }

        private IEnumerable<Affiliate> GetAffiliateUserCounts(IQueryable<Affiliate> query)
        {
            query = query.Include(a => a.Users);

            var affiliates = query.Select(a => new AffiliatePresentation
            {
                Affiliate = a,
                Logos = a.Logos,
                Features = a.Features,
                UserCount = a.Users.Count()
            }).ToList();

            foreach (var affiliate in affiliates)
            {
                affiliate.Affiliate.Logos = affiliate.Logos;
                affiliate.Affiliate.Features = affiliate.Features;
                affiliate.Affiliate.UserCount = affiliate.UserCount;
            }

            return affiliates.Select(g => g.Affiliate).OrderBy(a => a.Name);
        }

        #endregion
    }
}
