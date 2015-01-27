using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Portal.Data;
using Portal.Domain.Models;
using Portal.Infrastructure.Caching;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using Portal.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Portal.Domain.Services
{
    public class CmsService : ICmsService
    {
        #region Private Members

        private readonly ICmsRepository _cmsRepository;
        private readonly ICacheStorage _cacheStorage;

        #endregion

        #region Constructor

        public CmsService(ICmsRepository cmsRepository, ICacheStorage cacheStorage)
        {
            _cmsRepository = cmsRepository;
            _cacheStorage = cacheStorage;
        }

        #endregion

        #region Sites

        public Site GetSite(SiteRequest request)
        {
            var site = GetSites(request).FirstOrDefault();

            if (site != null)
            {
                site.SiteContents = site.SiteContents.Where(s => s.SiteContentStatusID != (int)ContentStatus.Removed)
                       .OrderBy(s => s.SiteContentParentID)
                       .ThenBy(s => s.SortOrder)
                       .ThenBy(s => s.Title)
                       .ToList();
            }

            return site;
        }

        public IEnumerable<Site> GetSites()
        {
            return _cmsRepository.GetAll<Site>().ToList();
        }

        public IEnumerable<Site> GetSites(SiteRequest request)
        {
            var query = _cmsRepository.GetAll<Site>();

            if (request.SiteID.HasValue)
                query = query.Where(s => s.SiteID == request.SiteID.Value);

            if (!string.IsNullOrEmpty(request.SiteName))
                query = query.Where(s => s.SiteName.Equals(request.SiteName, StringComparison.InvariantCultureIgnoreCase));

            if (request.IncludeAll)
            {
                query = query.Include("SiteTemplates")
                             .Include("SiteContents")
                             .Include("SiteContents.ProfileTypes")
                             .Include("SiteContents.Affiliates")
                             .Include("SiteContents.Versions")
                             .Include("SiteContents.Versions.SiteTemplate")
                             .Include("SiteContents.Children")
                             .Include("SiteContents.Parent");
            }
            else
            {
                if (request.IncludeSiteContents)
                    query = query.Include(s => s.SiteContents);

                if (request.IncludeSiteTemplates)
                    query = query.Include(s => s.SiteTemplates);
            }

            return query.ToList();
        }

        #endregion

        #region Site Contents

        public bool IsContentUrl(string permalink)
        {
            var url = permalink;

            if (!url.StartsWith("/"))
                url = "/" + url;

            var items = _cacheStorage.Retrieve("Cms.SiteContent", () =>
            {
                var contents = _cmsRepository.GetAll<SiteContent>().Where(s => s.SiteContentStatusID == (int) ContentStatus.Published).ToList();

                return contents.Where(s => s.IsPublished() && (s.IsContentPage() || s.IsFile())).ToList();
            });

            return items.Any(s => s.Permalink == url);
        }

        public SiteContentViewModel GetSiteContentViewModel(string permalink, int profileTypeId, int affiliateId)
        {
            var url = permalink;

            if (!url.StartsWith("/"))
                url = "/" + url;

            var content = GetSiteContentInternal(profileTypeId, affiliateId).FirstOrDefault(s => 
                                s.Permalink.Equals(url, StringComparison.InvariantCultureIgnoreCase) && s.IsPublished());

            if (content == null)
                return null;

            var viewModel = new SiteContentViewModel()
            {
                SiteID = content.Site.SiteID,
                SiteName = content.Site.SiteName,
                ContentHtml = content.ContentHtml,
                ContentScripts = content.ContentScripts,
                ContentStyles = content.ContentStyles,
                ModifyDate = content.ModifyDate,
                FileID = content.FileID ?? 0,
                ContentType = content.ContentType,
                DocumentType = content.DocumentType,
                TemplatePath = content.TemplatePath,
                Title = content.Title,
                SiteContentID = content.SiteContentID,
                SiteContentParentID = content.SiteContentParentID
            };

            if (content.IsPage())
            {
                var siteMap = GetSiteMap(content.SiteID, profileTypeId, affiliateId);

                var activePage = siteMap.Items.FindRecursive(s => s.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase));

                if (activePage != null)
                    activePage.IsActive = true;

                viewModel.SiteMap = siteMap;
            }

            return viewModel;
        }

        public IEnumerable<SiteContent> GetSiteContents(SiteContentRequest request)
        {
            var query = _cmsRepository.GetAll<SiteContent>()
                                .Include("Versions")
                                .Include("Versions.Affiliates");

            if (request.SiteContentID.HasValue)
                query = query.Where(s => s.SiteContentID == request.SiteContentID.Value);

            if (request.SiteID.HasValue)
                query = query.Where(s => s.SiteID == request.SiteID.Value);

            if (!string.IsNullOrEmpty(request.Permalink))
            {
                if (!request.Permalink.StartsWith("/"))
                    request.Permalink = "/" + request.Permalink;

                query = query.Where(s => s.Permalink == request.Permalink);
            }

            if (!request.ContentStatuses.IsNullOrEmpty())
                query = query.Where(s => request.ContentStatuses.Contains((ContentStatus)s.SiteContentStatusID));

            if (!request.ContentTypes.IsNullOrEmpty())
                query = query.Where(s => request.ContentTypes.Contains((ContentType)s.SiteContentTypeID));

            if (request.IncludeSite)
                query = query.Include("Site");

            if (request.IncludeSite && request.IncludeSiteContents)
                query = query.Include("Site.SiteContents");

            if (request.IncludeSite && request.IncludeSiteTemplates)
                query = query.Include("Site.SiteTemplates");

            if (request.IncludeProfileTypes)
                query = query.Include("ProfileTypes");

            if (request.IncludeAffiliates)
                query = query.Include("Affiliates");

            if (request.IncludeSiteDocumentType)
                query = query.Include("SiteDocumentType");

            if (request.IncludeSiteContentType)
                query = query.Include("SiteContentType");

            if (request.IncludeSiteContentStatus)
                query = query.Include("SiteContentStatus");

            if (request.IncludeSiteTemplates)
                query = query.Include("Versions.SiteTemplate");

            if (request.IncludeKnowledgeLibraries)
                query = query.Include("KnowledgeLibraries");

            if (request.IncludeSiteContentHistories)
                query = query.Include("SiteContentHistories");

            if (request.IncludeParents)
                query = query.Include("Parent");

            if (request.IncludeFileInfo)
                query = query.Include("FileInfo");

            if (!request.SearchTerms.IsNullOrEmpty())
            {
                query = query.AsNoTracking().ToList()
                            .Where(item => item.Title.Contains(request.SearchTerms.ToArray())
                                            || (item.DefaultContentVersion != null && item.DefaultContentVersion.ContentText.StripTags().Contains(request.SearchTerms.ToArray()))
                                            || item.Description.Contains(request.SearchTerms.ToArray()))
                            .AsQueryable();
            }

            return query.AsNoTracking().ToList();
        }

        public SiteContent GetSiteContent(SiteContentRequest request)
        {
            return GetSiteContents(request).FirstOrDefault();
        }

        public IEnumerable<SiteContentViewModel> SearchSiteContents(SiteContentRequest request)
        {
            if (request.SearchTerms.IsNullOrEmpty()) return null;

            var query = GetSiteContentInternal(request.ProfileTypeID, request.AffiliateID).Where(s => s.IsPublished());

            if (!request.ContentDocumentTypes.IsNullOrEmpty())
                query = query.Where(s => request.ContentDocumentTypes.Contains((ContentDocumentType)s.SiteDocumentTypeID));

            if (request.SiteID.HasValue && request.SiteID > 0)
                query = query.Where(s => s.SiteID == request.SiteID);

            if (!string.IsNullOrWhiteSpace(request.SiteName))
                query = query.Where(s => s.Site.SiteName.Equals(request.SiteName, StringComparison.InvariantCultureIgnoreCase));

            var results = FilterSearchResults(query.ToList())
                                .Where(item => item.Title.Contains(request.SearchTerms.ToArray())
                                               || item.ContentHtml.StripTags().Contains(request.SearchTerms.ToArray())
                                               || item.Description.Contains(request.SearchTerms.ToArray())).ToList();

            return results.Select(item =>
                {
                    var searchRank = request.SearchTerms.Sum(term =>
                            item.Title.CountOccurrencesOf(term) * 50
                            + item.Description.CountOccurrencesOf(term) * 2
                            + item.ContentHtml.CountOccurrencesOf(term) * 1);

                    var text = item.ContentHtml;

                    if (!string.IsNullOrEmpty(text) && request.MaxContentCharacters.HasValue)
                    {
                        var strippedText = Regex.Replace(text, @"\t|\n|\r|<br>|<br />|<br/>|<p>|</p>", " ").StripTags().Trim();
                        var retVal = strippedText.SubstringByWord(request.MaxContentCharacters.Value);

                        if (strippedText.Length > retVal.Length && retVal[retVal.Length - 1] != '.')
                        {
                            retVal += " ...";
                        }

                        text = retVal;
                    }

                    return new SiteContentViewModel()
                    {
                        Title = item.Title,
                        ContentHtml = text,
                        SiteName = item.Site.SiteName,
                        ContentType = item.ContentType,
                        DocumentType = item.DocumentType,
                        Url = item.Url,
                        SearchRank = searchRank
                    };
                });
        }

        public void SaveSiteContent(ref SiteContent content, int auditUserId)
        {
            // Ensure 0 values are set to null, otherwise we'll get FK errors
            if (content.SiteContentParentID <= 0)
                content.SiteContentParentID = null;

            // Clear Publish Date
            if (content.Status != ContentStatus.Published && content.PublishDateUtc.HasValue)
            {
                content.PublishDateUtc = null;
            }

            // Update audit fields
            if (content.SiteContentID <= 0)
            {
                content.SetAuditFields(auditUserId);
            }
            else
            {
                content.UpdateAuditFields(auditUserId);
            }

            // Ensure default content version
            if (content.IsContentPage() && content.Versions.IsNullOrEmpty())
            {
                var site = GetSite(new SiteRequest() {SiteID = content.SiteID, IncludeSiteTemplates = true});
                var template = site.SiteTemplates.First();

                if (site.DefaultSiteTemplateID.HasValue)
                {
                    template = site.SiteTemplates.First(s => s.SiteTemplateID == site.DefaultSiteTemplateID);
                }

                content.Versions.Add(new SiteContentVersion()
                {
                    SiteContentID = content.SiteContentID,
                    VersionName = "Default",
                    SiteTemplateID = template.SiteTemplateID,
                    ContentText = template.DefaultContent
                });
            }

            // Only content pages should have versions
            if (!content.IsContentPage() && !content.Versions.IsNullOrEmpty())
            {
                content.Versions.Clear();
            }

            // Ensure audit fields on versions
            foreach (var version in content.Versions.Where(version => version.SiteContentVersionID <= 0))
            {
                version.SetAuditFields(auditUserId);
            }

            // Save it
            _cmsRepository.SaveSiteContent(content);

            // Clear CMS-related cache
            _cacheStorage.ClearNamespace("Cms");
        }

        public PermalinkResponse GeneratePermalink(PermalinkRequest request)
        {
            var site = GetSite(new SiteRequest() { SiteID = request.SiteID, IncludeSiteContents = true});
            var slug = request.Title.Slugify();
            var permalink = site.ApplicationRootPath.TrimEnd('/');

            if (string.IsNullOrWhiteSpace(slug))
                throw new Exception("Invalid Title.  Cannot produce slug.");

            if (request.SiteContentParentID > 0)
            {
                var parent = site.SiteContents.FirstOrDefault(s => s.SiteContentID == request.SiteContentParentID);

                if (parent != null)
                {
                    permalink = parent.Permalink;
                }
            }

            // 'Namespace' the permalink to help reduce collisions
            switch ((ContentType)request.SiteContentTypeID)
            {
                case ContentType.File:
                    permalink += "/file";
                    break;
            }

            if (!permalink.EndsWith("/"))
                permalink += "/";

            permalink += slug;

            // Validate Permalink's uniqueness
            var version = 0;
            var original = permalink;

            while (true)
            {
                var exists = site.SiteContents.Any(s => s.Permalink.Equals(permalink, StringComparison.InvariantCultureIgnoreCase) && s.Status != ContentStatus.Removed);

                if (!exists)
                    break;

                permalink = original + "-" + ++version;
            }

            return new PermalinkResponse()
            {
                FullUrl = string.Concat("https://", site.DomainName, permalink),
                Slug = slug,
                Permalink = permalink
            };            
        }

        public IEnumerable<SiteContent> GetParentPages(int siteId, int excludeId = 0)
        {
            var request = new SiteRequest()
            {
                IncludeSiteContents = true,
                SiteID = siteId
            };

            var site = GetSite(request);
            var contents = site.SiteContents.Where(s => s.SiteContentTypeID != (int)ContentType.File && s.SiteContentStatusID != (int)ContentStatus.Removed).ToList();

            if (excludeId > 0)
            {
                return contents.GetPotentialParentPages(excludeId);
            }

            return contents.OrderByHierarchy();
        }

        private IEnumerable<SiteContent> GetSiteContentInternal(int profileTypeId, int affiliateId)
        {
            var cacheKey = string.Format("Cms.ProfileType-{0}.Affiliate-{1}", profileTypeId, affiliateId);

            return _cacheStorage.Retrieve(cacheKey, () =>
            {
                // Get all Published content
                var query = _cmsRepository.GetAll<SiteContent>().Where(s => s.SiteContentStatusID == (int) ContentStatus.Published);

                // Include everything possible
                query = query.Include("Versions")
                             .Include("Versions.SiteTemplate")
                             .Include("Versions.Affiliates")
                             .Include("Site")
                             .Include("Parent")
                             .Include("Children")
                             .Include("SiteContentStatus")
                             .Include("SiteDocumentType")
                             .Include("SiteContentType")
                             .Include("FileInfo")
                             .Include("Affiliates")
                             .Include("ProfileTypes")
                             .Include("KnowledgeLibraries");

                // Give it a known sort order
                var contents = query.OrderBy(s => s.SiteContentParentID).ThenBy(s => s.SortOrder).ThenBy(s => s.Title).ToList();

                // Parse the html of content pages
                Parallel.ForEach(contents.Where(s => s.IsContentPage()), content =>
                {
                    var version = content.GetAffiliateVersion(affiliateId) ?? content.DefaultContentVersion;

                    var parsedContent = ParseContent(version, contents, profileTypeId);

                    if (parsedContent != null)
                    {
                        content.ContentHtml = parsedContent.Html;
                        content.ContentStyles = parsedContent.Styles;
                        content.ContentScripts = parsedContent.Scripts;
                    }

                    if (version.SiteTemplate != null)
                        content.TemplatePath = version.SiteTemplate.LayoutPath;
                });

                // Return subset based on ProfileType and Affiliate
                return contents.Where(s => 
                                     (!s.ProfileTypes.Any() || s.ProfileTypes.Any(p => p.ProfileTypeID == profileTypeId)) &&
                                     (!s.Affiliates.Any() || s.Affiliates.Any(a => a.AffiliateID == affiliateId))).ToList();
            });

        }

        private static ParsedContent ParseContent(SiteContentVersion content, List<SiteContent> contents, int profileTypeId)
        {
            if (content == null || string.IsNullOrWhiteSpace(content.ContentText))
                return null;

            var parsedContent = new ParsedContent()
            {
                Html = string.Empty,
                Scripts = string.Empty,
                Styles = string.Empty
            };

            var doc = new HtmlDocument();
            doc.LoadHtml(content.ContentText);

            foreach (var script in doc.DocumentNode.Descendants("script").ToArray())
            {
                parsedContent.Scripts += script.InnerHtml;
                script.Remove();
            }

            foreach (var style in doc.DocumentNode.Descendants("style").ToArray())
            {
                parsedContent.Styles += style.InnerHtml;
                style.Remove();
            }

            foreach (var link in doc.DocumentNode.Descendants("a").ToArray())
            {
                var href = link.GetAttributeValue("href", string.Empty);

                if (!string.IsNullOrEmpty(href))
                {
                    var linkContent = contents.FindByPermalinkOrUrl(href);

                    if (linkContent != null)
                    {
                        if (linkContent.ProfileTypes.Any() && linkContent.ProfileTypes.All(p => p.ProfileTypeID != profileTypeId))
                        {
                            link.Attributes["href"].Remove();
                            link.SetAttributeValue("class", "disabled");
                        }
                        else if (linkContent.ContentType == ContentType.File && linkContent.Site != null && linkContent.FileInfo != null)
                        {
                            link.SetAttributeValue("data-track", "true");
                            link.SetAttributeValue("data-label", linkContent.Site.SiteName);
                            link.SetAttributeValue("data-value", linkContent.FileInfo.Name);
                        }
                    }
                }
            }

            parsedContent.Html = doc.DocumentNode.InnerHtml;

            return parsedContent;
        }

        private static IEnumerable<SiteContent> FilterSearchResults(IEnumerable<SiteContent> results)
        {
            var filteredResults = new List<SiteContent>();

            foreach (var item in results)
            {
                var parentId = item.SiteContentParentID;
                var accessible = true;

                while (parentId != null)
                {
                    var parent = results.FirstOrDefault(s => s.SiteContentID == parentId);

                    if (parent != null)
                    {
                        parentId = parent.SiteContentParentID;
                    }
                    else
                    {
                        accessible = false;
                        parentId = null;
                    }
                }

                if (accessible)
                    filteredResults.Add(item);
            }

            return filteredResults;
        }

        #endregion

        #region Site Map

        public SiteMap GetSiteMap(int siteId, int profileTypeId, int affiliateId)
        {
            var contents = GetSiteContentInternal(profileTypeId, affiliateId)
                                    .Where(s => s.SiteID == siteId && s.IsPublished() && s.IsPage()).ToList();

            var firstItem = contents.FirstOrDefault();

            if (firstItem == null)
                throw new Exception("Site Content Does Not Exist");

            var siteMap = new SiteMap()
            {
                SiteID = firstItem.SiteID,
                SiteName = firstItem.Site.SiteName,
                SiteRootUrl = firstItem.Site.ApplicationRootPath,
                Items = contents.Where(s => s.SiteContentParentID == null).Select(content => GetSiteMapItem(content, contents)).ToList()
            };

            var contactPage = siteMap.Items.FindRecursive(s => s.Title.Equals("Contact", StringComparison.InvariantCultureIgnoreCase));

            if (contactPage != null)
                siteMap.SiteContactUrl = contactPage.Url;

            var termsPage = siteMap.Items.FindRecursive(s => s.Title.Equals("Terms of Use", StringComparison.InvariantCultureIgnoreCase));

            if (termsPage != null)
                siteMap.SiteTermsUrl = termsPage.Url;

            return siteMap;
        }

        private SiteMapItem GetSiteMapItem(SiteContent content, List<SiteContent> contents)
        {
            var item = new SiteMapItem()
            {
                SiteContentID = content.SiteContentID,
                SiteContentParentID = content.SiteContentParentID,
                Title = content.Title,
                Url = content.Permalink,
                IconCssClass = content.MenuIconCssClass,
                IsMenuVisible = content.MenuVisible
            };

            foreach (var childItem in contents.Where(s => s.SiteContentParentID == content.SiteContentID).Select(child => GetSiteMapItem(child, contents)))
            {
                item.Children.Add(childItem);
            }

            return item;
        }

        #endregion

        #region Lookups

        public IEnumerable<MenuIcon> GetMenuIcons()
        {
            return _cmsRepository.GetAll<MenuIcon>().OrderBy(m => m.IconName).ToList();
        }

        #endregion

        #region Knowledge Library

        public IEnumerable<KnowledgeLibrary> GetKnowledgeLibrary(int siteId)
        {
            return _cmsRepository.GetAll<KnowledgeLibrary>()
                                .Where(kl => kl.SiteContent.SiteID == siteId && kl.SiteContent.SiteContentStatusID != (int)ContentStatus.Removed)
                                .Include("SiteContent")
                                .Include("SiteContent.ProfileTypes")
                                .Include("SiteContent.Affiliates")
                                .OrderBy(kl => kl.SiteContent.Title)
                                .ToList();
        }

        public IEnumerable<KnowledgeLibraryTopic> GetKnowledgeLibraryTopics(int siteId)
        {
            return _cmsRepository.FindBy<KnowledgeLibraryTopic>(k => k.SiteID == siteId).ToList();
        }

        #endregion

        #region Third Party Resources

        public IEnumerable<ThirdPartyResource> GetThirdPartyResources()
        {
            return _cmsRepository.GetAll<ThirdPartyResource>().Include(t => t.Affiliates).OrderBy(t => t.Name).ToList();
        }

        public IEnumerable<ThirdPartyResourceService> GetThirdPartyResourceServices()
        {
            return _cmsRepository.GetAll<ThirdPartyResourceService>().OrderBy(s => s.ServiceName).ToList();
        }

        public ThirdPartyResource GetThirdPartyResource(int thirdPartyResourceId)
        {
            return _cmsRepository.FindBy<ThirdPartyResource>(t => t.ThirdPartyResourceID == thirdPartyResourceId)
                        .Include((t => t.Affiliates))
                        .FirstOrDefault();
        }

        public void SaveThirdPartyResource(ref ThirdPartyResource resource)
        {
            var updatedResource = _cmsRepository.SaveThirdPartyResource(resource);

            resource = updatedResource;
        }

        public void DeleteThirdPartyResource(int thirdPartyResourceId)
        {
            var resource = _cmsRepository.FindBy<ThirdPartyResource>(r => r.ThirdPartyResourceID == thirdPartyResourceId).FirstOrDefault();

            if (resource != null)
            {
                _cmsRepository.Delete(resource);
                _cmsRepository.Save();
            }
        }

        #endregion
    }
}
