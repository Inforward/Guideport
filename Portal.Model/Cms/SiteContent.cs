using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Portal.Model
{
    public class SiteContent : Auditable
    {
        public SiteContent()
        {
            Children = new List<SiteContent>();
            KnowledgeLibraries = new List<KnowledgeLibrary>();
            ProfileTypes = new List<ProfileType>();
            Affiliates = new List<Affiliate>();
            Versions = new List<SiteContentVersion>();
        }

        public int SiteContentID { get; set; }
        public int? SiteContentParentID { get; set; }
        public int SiteID { get; set; }
        public int SiteContentStatusID { get; set; }
        public int SiteContentTypeID { get; set; }
        public int SiteDocumentTypeID { get; set; }
        public int? FileID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Permalink { get; set; }
        public int SortOrder { get; set; }
        public bool MenuVisible { get; set; }
        public string MenuIconCssClass { get; set; }
        public string MenuTarget { get; set; }
        public DateTime? PublishDateUtc { get; set; }
        public List<KnowledgeLibrary> KnowledgeLibraries { get; set; }
        public ICollection<ProfileType> ProfileTypes { get; set; }
        public ICollection<Affiliate> Affiliates { get; set; }
        public Site Site { get; set; }
        public FileInfo FileInfo { get; set; }
        public SiteContentType SiteContentType { get; set; }
        public SiteDocumentType SiteDocumentType { get; set; }
        public SiteContentStatus SiteContentStatus { get; set; }
        public List<SiteContentVersion> Versions { get; set; }

        [IgnoreDataMember]
        public ICollection<SiteContent> Children { get; set; }

        [IgnoreDataMember]
        public SiteContent Parent { get; set; }

        [NotMapped]
        public int SearchRank { get; set; }

        [NotMapped]
        public ContentType ContentType
        {
            get { return (ContentType)SiteContentTypeID; }
        }

        [NotMapped]
        public ContentDocumentType DocumentType
        {
            get { return (ContentDocumentType)SiteDocumentTypeID; }
        }

        [NotMapped]
        public ContentStatus Status
        {
            get { return (ContentStatus)SiteContentStatusID; }
        }

        [NotMapped]
        [IgnoreDataMember]
        public KnowledgeLibrary KnowledgeLibrary
        {
            get { return KnowledgeLibraries.Count > 0 ? KnowledgeLibraries.FirstOrDefault() : null; }
        }

        [NotMapped]
        public string TitlePath
        {
            get
            {
                if (Parent != null)
                {
                    return Parent.TitlePath + " > " + Title;
                }

                return Title ?? string.Empty;
            }
        }

        [NotMapped]
        public string Url
        {
            get
            {
                var url = string.Empty;

                if (Site != null && !string.IsNullOrWhiteSpace(Permalink))
                {
                    url = string.Concat("https://", Site.DomainName, Permalink);
                }

                return url;
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        public SiteContentVersion DefaultContentVersion
        {
            get
            {
                if (Versions == null || !Versions.Any())
                    return null;

                return Versions.FirstOrDefault(v => v.VersionName == "Default");
            }
        }

        [NotMapped]
        public string ContentHtml { get; set; }

        [NotMapped]
        public string ContentStyles { get; set; }

        [NotMapped]
        public string ContentScripts { get; set; }

        [NotMapped]
        public string TemplatePath { get; set; }
    }

    public static class SiteContentExtensions
    {
        public static SiteContentVersion GetAffiliateVersion(this SiteContent content, int affiliateId)
        {
            if (content == null || content.Versions == null)
                return null;

            return content.Versions.FirstOrDefault(v => v.Affiliates.Any(a => a.AffiliateID == affiliateId));
        }

        public static bool IsPage(this SiteContent content)
        {
            if (content == null) return false;

            return content.ContentType == ContentType.ContentPage || content.ContentType == ContentType.StaticPage;
        }

        public static bool IsFile(this SiteContent content)
        {
            if (content == null) return false;

            return content.ContentType == ContentType.File;
        }

        public static bool IsContentPage(this SiteContent content)
        {
            if (content == null) return false;

            return content.ContentType == ContentType.ContentPage;
        }

        public static bool IsPublished(this SiteContent content)
        {
            if (content == null) return false;

            return content.SiteContentStatusID == (int) ContentStatus.Published && content.PublishDateUtc <= DateTime.UtcNow;
        }

        public static IEnumerable<SiteContent> GetPages(this IEnumerable<SiteContent> siteContents)
        {
            return siteContents.Where(c => c.IsPage());
        }

        public static IEnumerable<SiteContent> GetPublishedPages(this IEnumerable<SiteContent> siteContents)
        {
            return siteContents.Where(c => c.IsPage() && c.IsPublished());
        }

        public static IEnumerable<SiteContent> GetRootPages(this IEnumerable<SiteContent> siteContents)
        {
            return siteContents.GetPublishedPages().Where(s => s.SiteContentParentID == null && s.MenuVisible);
        }

        public static SiteContent FindByPermalink(this IEnumerable<SiteContent> siteContents, string permalink)
        {
            if (!permalink.StartsWith("/"))
                permalink = "/" + permalink;

            return siteContents.FirstOrDefault(s => s.Permalink != null && s.Permalink.Equals(permalink, StringComparison.InvariantCultureIgnoreCase) && s.SiteContentStatusID != (int)ContentStatus.Removed);
        }

        public static SiteContent FindByPermalinkOrUrl(this IEnumerable<SiteContent> siteContents, string href)
        {
            var content = siteContents.FirstOrDefault(s => s.Url != null && s.Url.Equals(href, StringComparison.InvariantCultureIgnoreCase) && s.SiteContentStatusID != (int)ContentStatus.Removed);

            if (content != null)
                return content;

            var permalink = href;

            if (!permalink.StartsWith("/"))
                permalink = "/" + permalink;

            return siteContents.FirstOrDefault(s => s.Permalink != null && s.Permalink.Equals(permalink, StringComparison.InvariantCultureIgnoreCase) && s.SiteContentStatusID != (int)ContentStatus.Removed);
        }

        public static IEnumerable<SiteContent> GetPotentialParentPages(this IEnumerable<SiteContent> siteContents, int excludeContentId)
        {
            var rootList = siteContents.GetPages().Where(s => s.SiteContentParentID == null && s.Status != ContentStatus.Removed && s.SiteContentID != excludeContentId).OrderBy(s => s.SortOrder).ToList();

            return OrderByHierarchyInternal(rootList, excludeContentId);
        }

        public static IEnumerable<SiteContent> OrderByHierarchy(this IEnumerable<SiteContent> siteContents)
        {
            var rootList = siteContents.Where(s => s.SiteContentParentID == null && s.Status != ContentStatus.Removed);

            return OrderByHierarchyInternal(rootList);
        }

        private static IEnumerable<SiteContent> OrderByHierarchyInternal(IEnumerable<SiteContent> siteContents, int? excludeContentId = null)
        {
            var list = new List<SiteContent>();

            foreach (var content in siteContents.Where(s => !excludeContentId.HasValue || s.SiteContentID != excludeContentId && s.Status != ContentStatus.Removed).OrderBy(s => s.SortOrder))
            {
                list.Add(content);

                if (content.Children.Any())
                {
                    var items = OrderByHierarchyInternal(content.Children, excludeContentId).ToList();

                    if (items.Any())
                    {
                        list.AddRange(items);
                    }
                }
            }

            return list;
        }
    }
}
