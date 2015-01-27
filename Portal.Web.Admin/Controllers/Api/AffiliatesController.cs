using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.Admin.Models;
using Portal.Web.Common.Filters.Api;

namespace Portal.Web.Admin.Controllers
{
    [RoutePrefix("api/affiliates")]
    public class AffiliatesController : BaseApiController
    {
        private readonly IAffiliateService _affiliateService;
        private readonly IFileService _fileService;

        public AffiliatesController(IAffiliateService affiliateService, IUserService userService, ILogger logger, IFileService fileService) 
            : base(userService, logger)
        {
            _affiliateService = affiliateService;
            _fileService = fileService;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public IEnumerable<dynamic> GetAffiliates([FromUri]AffiliateRequest request)
        {
            request = request ?? new AffiliateRequest();

            return _affiliateService.GetAffiliates(request).OrderBy(a => a.Name).Select(a => new
                {
                    a.AffiliateID,
                    a.Name,
                    a.ExternalID,
                    Phone = a.Phone.FormatAsPhoneNo(),
                    a.WebsiteUrl,
                    Address = a.Address.Replace(Environment.NewLine, "<br/>"),
                    a.UserCount,
                    a.ModifyDateUtc
                });
        }

        [HttpGet]
        [Route("{id:int}")]
        public Affiliate GetAffiliate(int id)
        {
            var request = new AffiliateRequest
            {
                AffiliateID = id, 
                UseCache = false,
                IncludeLogos = true
            };

            return _affiliateService.GetAffiliates(request).FirstOrDefault();
        }

        [HttpGet]
        [Route("{id:int}/features")]
        public IEnumerable<FeatureViewModel> GetAffiliateFeatures(int id)
        {
            var list = new List<FeatureViewModel>();

            foreach (var feature in _affiliateService.GetAffiliateFeatures(id))
            {
                var featureViewModel = new FeatureViewModel()
                {
                    AffiliateFeatureID = feature.AffiliateFeatureID,
                    AffiliateID = feature.AffiliateID,
                    FeatureID = feature.FeatureID,
                    IsEnabled = feature.IsEnabled,
                    Name = feature.Feature.Name,
                    Tooltip = feature.Feature.Description
                };

                foreach (var setting in feature.Settings)
                {
                    var rawSetting = feature.Feature.Settings.First(s => s.FeatureSettingID == setting.FeatureSettingID);

                    featureViewModel.Settings.Add(new FeatureSettingViewModel()
                    {
                        AffiliateFeatureID = feature.AffiliateFeatureID,
                        FeatureSettingID = setting.FeatureSettingID,
                        Value = setting.Value,
                        Name = rawSetting.Name,
                        Placeholder = rawSetting.PlaceholderValue,
                        Tooltip = rawSetting.Description,
                        VisibleState = rawSetting.VisibleState,
                        IsRequired = rawSetting.IsRequired,
                        ValidationRegEx = rawSetting.ValidationRegEx
                    });
                }

                list.Add(featureViewModel);
            }

            return list;
        }

        [HttpGet]
        [Route("{id:int}/objectives")]
        public IEnumerable<AffiliateObjective> GetAffiliateObjectives(int id)
        {
            return _affiliateService.GetObjectives(id);
        }

        [HttpPost]
        [Route("{id:int}/logo/{logoTypeId:int}")]
        public AffiliateLogo UploadLogo(int id, int logoTypeId)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var request = HttpContext.Current.Request;

            if (request.Files.Count <= 0)
                throw new Exception("No files found for upload");

            var file = request.Files[0];
            var affiliate = GetAffiliate(id);

            if(affiliate == null)
                throw new Exception("Invalid Affiliate ID");

            var logo = affiliate.Logos.FirstOrDefault(l => l.AffiliateLogoTypeID == logoTypeId);

            if (logo == null)
                throw new Exception("Could not find logo type to update");

            UpdateAuditData(logo);

            var extension = Path.GetExtension(file.FileName).Substring(1);
            var fileName = string.Format("{0}-{1}.{2}", affiliate.Name.Slugify(), logo.LogoType.Name.Slugify(), extension);

            // Upload the logo
            var fileInfo = _fileService.UploadFile(new Model.File()
            {
                Stream = file.InputStream,
                Info = new Model.FileInfo()
                {
                    Name = fileName,
                    Extension = Path.GetExtension(file.FileName).Substring(1),
                    SizeBytes = file.ContentLength,
                    CreateUserID = CurrentUser.UserID,
                    CreateDate = DateTime.Now
                }
            });

            if (fileInfo == null)
                throw new Exception("Could not upload to file service");

            // Set the file reference on the logo
            logo.FileID = fileInfo.FileID;
            logo.FileInfo = fileInfo;

            // Save it
            _affiliateService.UpdateAffiliateLogo(logo);

            return logo;
        }

        [HttpPut]
        [Route("{id:int}/features")]
        [PortalAuthorize(PortalRoleValues.AffiliateAdmin)]
        public IEnumerable<FeatureViewModel> UpdateAffiliateFeatures(int id, [FromBody] IEnumerable<FeatureViewModel> features)
        {
            var existingFeatures = _affiliateService.GetAffiliateFeatures(id).ToList();

            foreach (var feature in features)
            {
                var existingFeature = existingFeatures.FirstOrDefault(f => f.FeatureID == feature.FeatureID);

                if (existingFeature != null)
                {
                    existingFeature.IsEnabled = feature.IsEnabled;

                    foreach (var setting in feature.Settings)
                    {
                        var existingSetting = existingFeature.Settings.FirstOrDefault(s => s.FeatureSettingID == setting.FeatureSettingID);

                        if (existingSetting != null)
                        {
                            existingSetting.Value = setting.Value;
                        }
                    }

                    UpdateAuditData(existingFeature);
                }
            }

            _affiliateService.UpdateAffiliateFeatures(id, existingFeatures);

            return GetAffiliateFeatures(id);
        }
            
        [HttpPut]
        [Route("{id:int}")]
        [PortalAuthorize(PortalRoleValues.AffiliateAdmin)]
        public Affiliate UpdateAffiliate([FromBody]Affiliate affiliate)
        {
            UpdateAuditData(affiliate);

            _affiliateService.UpdateAffiliate(ref affiliate);

            return affiliate;
        }

        [HttpPut]
        [Route("{id:int}/objectives")]
        [PortalAuthorize(PortalRoleValues.AffiliateAdmin)]
        public void UpdateAffiliateObjectives(int id, [FromBody] IEnumerable<AffiliateObjective> objectives)
        {
            var affiliateObjectives = objectives as IList<AffiliateObjective> ?? objectives.ToList();

            affiliateObjectives.ForEach(UpdateAuditData);

            _affiliateService.UpdateAffiliateObjectives(id, affiliateObjectives);
        }

    }
}