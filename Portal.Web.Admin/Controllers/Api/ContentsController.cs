using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using Portal.Web.Admin.Models;
using Portal.Web.Common.Filters.Api;
using Portal.Web.Common.Helpers;

namespace Portal.Web.Admin.Controllers.Api
{
    [RoutePrefix("api/contents")]
    [PortalAuthorize(PortalRoleValues.ContentAdmin)]
    public class ContentsController : BaseApiController
    {
        private readonly ICmsService _cmsService;
        private readonly IFileService _fileService;

        public ContentsController(IUserService userService, ILogger logger, ICmsService cmsService, IFileService fileService) 
            : base(userService, logger)
        {
            _cmsService = cmsService;
            _fileService = fileService;
        }

        [HttpGet]
        [Route("pages")]
        public dynamic GetPages([FromUri]SearchContentRequest searchRequest)
        {
            var request = new SiteContentRequest()
            {
                SiteID = searchRequest.SiteID,
                ContentTypes = new List<ContentType>() {  ContentType.ContentPage, ContentType.StaticPage },
                IncludeSiteContentStatus = true,
                IncludeSiteContentType = true
            };

            if (!string.IsNullOrWhiteSpace(searchRequest.SearchText))
            {
                request.SearchTerms.Add(searchRequest.SearchText);
            }

            if (searchRequest.SiteContentStatusID > 0)
            {
                request.ContentStatuses.Add((ContentStatus) searchRequest.SiteContentStatusID);
            }
            else
            {
                request.ContentStatuses.AddRange(new[] { ContentStatus.Published, ContentStatus.Draft });
            }

            var content = _cmsService.GetSiteContents(request).OrderBy(s => s.SiteContentParentID).ThenBy(s => s.SortOrder).ToList();

            return content.Select(s => new
                {
                    s.SiteContentID,
                    s.SiteContentParentID,
                    s.Title,
                    s.TitlePath,
                    s.SiteID,
                    s.SiteContentStatus.StatusDescription,
                    s.SiteContentType.ContentTypeDescription,
                    s.SiteContentType.ContentTypeName,
                    s.ModifyDateUtc,
                    s.Permalink,
                    VersionCount = s.Versions.Count
                });
        }

        [HttpGet]
        [Route("pages/{siteId}/parents/{excludeId:int?}")]
        public IEnumerable<dynamic> GetParentPages(int siteId, int excludeId = 0)
        {
            var pages = _cmsService.GetParentPages(siteId, excludeId).ToList();
            var list = new Dictionary<int, string>();

            foreach (var page in pages)
            {
                var current = page;
                var path = string.Empty;

                while (current != null)
                {
                    if (path.Length > 0)
                        path = " > " + path;

                    path = current.Title + path;

                    current = current.SiteContentParentID.HasValue ? pages.FirstOrDefault(c => c.SiteContentID == current.SiteContentParentID) : null;
                }

                list.Add(page.SiteContentID, path);
            }

            return list.Select(item => new
            {
                SiteContentID = item.Key,
                Path = item.Value
            });
        }

        [HttpGet]
        [Route("files")]
        public dynamic GetFiles([FromUri]SearchContentRequest searchRequest)
        {
            var request = new SiteContentRequest()
            {
                SiteID = searchRequest.SiteID,
                ContentTypes = new List<ContentType>() { ContentType.File },
                ContentStatuses = new List<ContentStatus>() { ContentStatus.Published },
                IncludeSiteDocumentType = true,
                IncludeKnowledgeLibraries = true,
                IncludeFileInfo = true
            };

            if (!string.IsNullOrWhiteSpace(searchRequest.SearchText))
            {
                request.SearchTerms.Add(searchRequest.SearchText);
            }

            var content = _cmsService.GetSiteContents(request).OrderByDescending(s => s.ModifyDateUtc).ToList();

            return content.Select(s => new
                {
                    s.SiteContentID,
                    s.Title,
                    s.SiteID,
                    FileName = s.FileInfo != null ? s.FileInfo.Name : null,
                    FileSize = s.FileInfo != null ? s.FileInfo.SizeKiloBytes : 0,
                    FileType = s.FileInfo != null ? s.FileInfo.Extension.TrimStart('.').ToUpper() : null,
                    KnowledgeLibrary = s.KnowledgeLibrary != null ? "Yes" : null,
                    s.ModifyDateUtc,
                    s.Permalink
                });
        }

        [HttpPost]
        [Route("files/upload")]
        public SiteContent UploadFile()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var request = HttpContext.Current.Request;

            if (request.Files.Count <= 0)
                throw new Exception("No files found for upload");

            var file = request.Files[0];
            var content = default(SiteContent);
            var contentId = request.Form.GetValue<int>("SiteContentID");

            if (contentId > 0)
            {
                content = GetContent(contentId);

                content.Title = request.Form.GetValue<string>("Title");
                content.Description = request.Form.GetValue<string>("Description");
            }
            else
            {
                content = new SiteContent()
                {
                    SiteID = request.Form.GetValue<int>("SiteID"),
                    Permalink = request.Form.GetValue<string>("Permalink"),
                    Title = request.Form.GetValue<string>("Title"),
                    Description = request.Form.GetValue<string>("Description"),
                    SiteContentStatusID = (int)ContentStatus.Published,
                    SiteContentTypeID = (int)ContentType.File,
                    SiteDocumentTypeID = (int)Path.GetExtension(file.FileName).Substring(1).MapFromCodeTo(ContentDocumentType.Pinion),
                    PublishDateUtc = DateTime.UtcNow
                };
            }

            if (content.Description == "null")
                content.Description = null;

            if(string.IsNullOrWhiteSpace(content.Title) || content.Title == "null")
                throw new Exception("Invalid file Title");


            // Upload the file
            var fileInfo = _fileService.UploadFile(new Model.File()
            {
                Stream = file.InputStream,
                Info = new Model.FileInfo()
                {
                    Name = file.FileName,
                    Extension = Path.GetExtension(file.FileName).Substring(1),
                    SizeBytes = file.ContentLength,
                    CreateUserID = CurrentUser.UserID,
                    CreateDate = DateTime.Now
                }
            });

            if (fileInfo == null)
                throw new Exception("Could not upload to file service");

            content.FileID = fileInfo.FileID;

            _cmsService.SaveSiteContent(ref content, CurrentUser.UserID);

            return GetContent(content.SiteContentID);
        }

        [HttpGet]
        [Route("{id:int}")]
        public SiteContent GetContent(int id)
        {
            var request = new SiteContentRequest()
            {
                SiteContentID = id,
                IncludeSiteContentStatus = true,
                IncludeSiteContentType = true,
                IncludeSiteDocumentType = true,
                IncludeProfileTypes = true,
                IncludeAffiliates = true,
                IncludeSite = true,
                IncludeSiteTemplates = true,
                IncludeVersions = true,
                IncludeFileInfo = true,
                IncludeKnowledgeLibraries = true
            };

            var content = _cmsService.GetSiteContent(request);

            if(content == null)
                throw new Exception("Invalid Site Content ID");

            return content;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public void TrashContent(int id)
        {
            var content = _cmsService.GetSiteContent(new SiteContentRequest() { SiteContentID = id });

            if (content == null)
                throw new Exception("Invalid Site Content ID");

            content.SiteContentStatusID = (int)ContentStatus.Removed;

            _cmsService.SaveSiteContent(ref content, CurrentUser.UserID);
        }

        [HttpPut]
        [Route("{id:int}")]
        public dynamic UpdateContent(int id, [FromBody] SiteContent content)
        {
            _cmsService.SaveSiteContent(ref content, CurrentUser.UserID);

            return GetContent(content.SiteContentID);
        }

        [HttpPost]
        [Route("")]
        public dynamic CreateContent([FromBody] SiteContent content)
        {
            _cmsService.SaveSiteContent(ref content, CurrentUser.UserID);

            return GetContent(content.SiteContentID);
        }

        [HttpPost]
        [Route("{id:int}/versions")]
        public SiteContentVersion CreateContentVersion([FromBody] SiteContentVersion version)
        {
            var fetchContent = new Func<SiteContent>(() => _cmsService.GetSiteContent(new SiteContentRequest()
                {
                    SiteContentID = version.SiteContentID,
                    IncludeProfileTypes = true,
                    IncludeAffiliates = true,
                    IncludeVersions = true
                }));

            var content = fetchContent();

            if (content == null)
                throw new Exception("Invalid Site Content ID");

            if (content.ContentType != ContentType.ContentPage)
                throw new Exception("Only content page types can have versions");

            if (content.Versions.Any(v => v.VersionName.Equals(version.VersionName, StringComparison.InvariantCultureIgnoreCase)))
                throw new Exception("A version with this name already exists");

            content.Versions.Add(version);

            _cmsService.SaveSiteContent(ref content, CurrentUser.UserID);

            return fetchContent().Versions.First(v => v.VersionName == version.VersionName);
        }

        [HttpPut]
        [Route("{id:int}/versions/{versionId:int}")]
        public void UpdateContentVersion(int id, int versionId, [FromBody] SiteContentVersion version)
        {
            var content = _cmsService.GetSiteContent(new SiteContentRequest()
                {
                    SiteContentID = version.SiteContentID,
                    IncludeProfileTypes = true,
                    IncludeAffiliates = true,
                    IncludeVersions = true
                });

            if (content == null)
                throw new Exception("Invalid Site Content ID");

            if (content.Versions.Any(v => v.VersionName.Equals(version.VersionName, StringComparison.InvariantCultureIgnoreCase) && v.SiteContentVersionID != version.SiteContentVersionID))
                throw new Exception("A version with this name already exists");

            content.Versions.Remove(version);
            content.Versions.Add(version);

            _cmsService.SaveSiteContent(ref content, CurrentUser.UserID);
        }

        [HttpDelete]
        [Route("{id:int}/versions/{versionId:int}")]
        public void DeleteContentVersion(int id, int versionId)
        {
            var request = new SiteContentRequest()
            {
                SiteContentID = id,
                IncludeProfileTypes = true,
                IncludeAffiliates = true,
                IncludeVersions = true
            };

            var content = _cmsService.GetSiteContent(request);

            if (content == null)
                throw new Exception("Invalid Site Content ID");

            var version = content.Versions.FirstOrDefault(v => v.SiteContentVersionID == versionId);

            if (version != null)
            {
                content.Versions.Remove(version);

                _cmsService.SaveSiteContent(ref content, CurrentUser.UserID);
            }
        }

        [HttpGet]
        [Route("permalinks")]
        public PermalinkResponse GeneratePermalink([FromUri]PermalinkRequest request)
        {
            return _cmsService.GeneratePermalink(request);
        }

        [HttpPut]
        [Route("permalinks")]
        public void ValidatePermalink([FromBody]SiteContent content)
        {
            var existingContent = _cmsService.GetSiteContent(new SiteContentRequest() { Permalink = content.Permalink });

            if (existingContent != null && existingContent.SiteContentStatusID != (int)ContentStatus.Removed &&  existingContent.SiteContentID != content.SiteContentID)
                throw new Exception("Permalink already exists");
        }

        [HttpGet]
        [Route("sites")]
        public IEnumerable<Site> GetSites()
        {
            return _cmsService.GetSites();
        }

        [HttpGet]
        [Route("menu-icons")]
        public IEnumerable<MenuIcon> GetMenuIcons()
        {
            return _cmsService.GetMenuIcons();
        }

        [HttpGet]
        [Route("sites/{siteId:int}/topics")]
        public dynamic GetSiteTopics(int siteId)
        {
            var list = _cmsService.GetKnowledgeLibraryTopics(siteId).OrderBy(s => s.Topic).ThenBy(s => s.Subtopic).ToList();

            return new
            {
                Topics = list.Select(t => new { Text = t.Topic, t.Topic }).Distinct(),
                Subtopics = list.Select(t => new { t.Topic, t.Subtopic }).Distinct()
            };
        }

        [HttpGet]
        [Route("third-party-resources")]
        public IEnumerable<dynamic> GetThirdPartyResources()
        {
            var resources = _cmsService.GetThirdPartyResources();

            return resources.Select(t => new
            {
                t.ThirdPartyResourceID,
                t.Name,
                t.AddressHtml,
                t.ServicesHtml,
                t.Email,
                PhoneNo = t.PhoneNo.FormatAsPhoneNoWithExtension(t.PhoneNoExt),
                t.WebsiteUrl,
                t.ModifyDateUtc
            });
        }

        [HttpDelete]
        [Route("third-party-resources/{id:int}")]
        public void DeleteThirdPartyResource(int id)
        {
            _cmsService.DeleteThirdPartyResource(id);
        }

        [HttpGet]
        [Route("third-party-resources/{id:int}")]
        public ThirdPartyResource GetThirdPartyResource(int id)
        {
            var resource = _cmsService.GetThirdPartyResource(id);

            return resource;
        }

        [HttpPost]
        [Route("third-party-resources")]
        public ThirdPartyResource CreateThirdPartyResource([FromBody]ThirdPartyResource resource)
        {
            resource.SetAuditFields(CurrentUser.UserID);

            _cmsService.SaveThirdPartyResource(ref resource);

            return resource;
        }

        [HttpPut]
        [Route("third-party-resources/{id:int}")]
        public ThirdPartyResource UpdateThirdPartyResource(int id, [FromBody]ThirdPartyResource resource)
        {
            resource.UpdateAuditFields(CurrentUser.UserID);

            _cmsService.SaveThirdPartyResource(ref resource);

            return resource;
        }

        [HttpGet]
        [Route("third-party-resources/services")]
        public IEnumerable<string> GetThirdPartyResourceServices()
        {
            return _cmsService.GetThirdPartyResourceServices().Select(s => s.ServiceName).ToList();
        }
    }
}
