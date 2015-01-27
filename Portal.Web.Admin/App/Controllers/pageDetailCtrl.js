'use strict';

angular.module('admin.Controllers')
    .controller("pageDetailCtrl", ['$scope', '$routeParams', '$modal', '$timeout', '$q', 'contentService', 'userService', 'affiliateService', 'appConfig', function ($scope, $routeParams, $modal, $timeout, $q, contentService, userService, affiliateService, appConfig) {

    $scope.editingPermalink = false;
    $scope.profileTypes = [];
    $scope.affiliates = [];
    $scope.options = {};

    $scope.content = {
        SiteID: parseInt($routeParams.siteId),
        SiteContentID: $routeParams.id ? parseInt($routeParams.id) : null,
        SiteContentParentID: -1,
        SiteContentTypeID: 1,
        SiteContentStatusID: 2,
        SiteDocumentTypeID: 5,
        SortOrder: 1,
        MenuVisible: true,
        Permalink: '',
        CurrentPermalink: '',
        Title: '',
        PublishDateUtc: new Date()
    };

    $scope.siteChanged = function() {
        $scope.setSite();
        $scope.setPermalink();
    };

    $scope.setSite = function() {
        $scope.content.Site = _.find($scope.sites, function (s) { return s.SiteID == $scope.content.SiteID; });
        $scope.refreshParents();
    };

    $scope.setPublishDate = function() {
        if ($scope.content.SiteContentStatusID == 2 && $scope.content.PublishDateUtc)
            $scope.content.PublishDateUtc = null;

        if ($scope.content.SiteContentStatusID == 1 && !$scope.content.PublishDateUtc)
            $scope.content.PublishDateUtc = new Date();
    };

    $scope.refreshParents = function () {
        var content = $scope.content;

        contentService.getParents(content.SiteID, content.SiteContentID).then(function (data) {
            $scope.parentPages = data;
            $scope.parentPages.unshift({ 'SiteContentID': -1, 'TitlePath': '(no parent)' });
        });
    };

    $scope.isNew = function () {
        return $scope.content.SiteContentID == null;
    };

    $scope.updateContent = function (content) {
        
        $scope.content = content;
        $scope.title = $scope.content.Title;
        $scope.refreshParents();

        if ($scope.content.PublishDateUtc)
            $scope.content.PublishDateUtc = new Date($scope.content.PublishDateUtc);

        if ($scope.content.SiteContentParentID == null)
            $scope.content.SiteContentParentID = -1;

        // Re-select version
        if ($scope.options.Version) {
            var version = _.find($scope.content.Versions, function(v) { return $scope.options.Version.SiteContentVersionID == v.SiteContentVersionID; });
            if (version)
                $scope.options.Version = version;
        } else {
            $scope.options.Version = $scope.content.Versions[0];
        }
    };

    $scope.savePage = function (form) {
        $scope.isSaving = true;

        contentService.saveContent($scope.content).then(function(response) {
            $scope.updateContent(response);
            $scope.isSaving = false;
            form.$setPristine();
        });
    };

    $scope.previewVersion = function() {
        var version = $scope.options.Version,
            content = $scope.content;

        contentService.saveContent(content).then(function () {
            window.open(appConfig.GuideportUrl + "/content/" + content.SiteContentID + "/preview/" + version.SiteContentVersionID);
        });
    };

    $scope.editPermalink = function() {
        $scope.editingPermalink = true;
        $scope.content.CurrentPermalink = $scope.content.Permalink;
    }

    $scope.cancelPermalinkEdit = function() {
        $scope.editingPermalink = false;
    };

    $scope.setPermalink = function () {
        if (!$scope.content.Title.length) {
            if ($scope.isNew()) $scope.content.Permalink = '';
            return;
        }

        if ($scope.isNew()) {
            contentService.generatePermalink($scope.content).then(function(response) {
                $scope.content.Permalink = response.Permalink;
            });
        }
    };

    $scope.changePermalink = function() {
        var content = $scope.content,
            permalink = content.CurrentPermalink,
            valid = permalink.match(/^((?:\/[a-zA-Z0-9]+(?:_[a-zA-Z0-9]+)*(?:\-[a-zA-Z0-9]+)*)+)$/);

        if (!valid) {
            var messages = ["Must begin with a forward slash", "Cannot end with a forward slash", "Segments may not lead or end with an underscore or dash", "Segments can not be doubled (__ or --)"];
            alert("Invalid Permalink. Permalinks:\n\n" + messages.join("\n"));
            return;
        }

        contentService.validatePermalink(permalink, content.SiteID, content.SiteContentID).then(function () {
            $scope.content.Permalink = permalink;
            $scope.cancelPermalinkEdit();
        });
    };

    $scope.menuIconOptions = {
        dataTextField: "IconName",
        dataValueField: "IconCssClass",
        valuePrimitive: true,
        autoBind: true,
        template: "<i class='menu-icon {{ dataItem.IconCssClass }}'></i> <span>{{ dataItem.IconName }}</span>",
        valueTemplate: "<i class='menu-icon {{ dataItem.IconCssClass }}'></i> <span>{{ dataItem.IconName }}</span>",
        dataSource: {
            transport: {
                read: function (e) {
                    contentService.getMenuIcons().then(function (data) {
                        data.unshift({ 'IconCssClass': '', 'IconName': '(none)' });
                        e.success(data);
                    });
                }
            }
        }
    };

    $scope.versionOptions = {
        dataSource: {
            transport: {
                read: function (e) {
                    e.success($scope.content.Versions || []);
                }
            }
        },
        change: function (e) {
            var id = e.sender.value();

            // Hack:  need to manually set this for some reason.  If not, the content changes won't persist.
            $timeout(function () {
                $scope.options.Version = _.find($scope.content.Versions, function (v) { return id == v.SiteContentVersionID; });
            });           
            
        },
        dataValueField: 'SiteContentVersionID',
        dataTextField: 'VersionName',
        valueTemplate: '{{ dataItem.VersionName }}',
        template: '<div class=\'content-version\'>' +
                    '<strong>{{ dataItem.VersionName }}</strong>' +
                    '<ul>' +
                    '#if(data && data.Affiliates && data.Affiliates.length)' +
                    '{' +
                        'for(var i=0;i<data.Affiliates.length;i++)' +
                        '{#' +
                            '<li>#=data.Affiliates[i].Name#</li>' +
                        '#}' +
                    '}#' +
                    '</ul>' +
                  '</div>'
    };

    $scope.addVersion = function () {
        var newVersion = {
            SiteContentID: $scope.content.SiteContentID,
            SiteTemplateID: $scope.content.Versions[0].SiteTemplateID,
            VersionName: '',
            Affiliates: []
        };

        $scope.openVersionDialog(newVersion).then(function (version) {
            if (version) {
                contentService.createContentVersion(version).then(function (data) {
                    $scope.content.Versions.push(data);
                    $scope.options.Version = $scope.content.Versions[$scope.content.Versions.length - 1];
                });
            }
        });
    };

    $scope.editVersion = function () {
        var version = angular.extend({}, $scope.options.Version);

        $scope.openVersionDialog(version).then(function (version) {
            if (version) {
                contentService.updateContentVersion(version).then(function () {
                    var index = -1;

                    $scope.content.Versions.forEach(function(item, idx) {
                        if (item.SiteContentVersionID == version.SiteContentVersionID)
                            index = idx;
                    });

                    if (index != -1) {
                        $scope.content.Versions[index] = version;
                        $scope.options.Version = $scope.content.Versions[index];
                    }
                });
            }
        });
    };

    $scope.openVersionDialog = function (currentVersion) {
        var deferred = $q.defer();

        var modalInstance = $modal.open({
            templateUrl: '/app/views/content/page-version-modal.html',
            controller: 'pageVersionCtrl',
            size: 'sm',
            resolve: {
                existingAffiliates: function () {
                    var affiliates = [];

                    $scope.content.Versions.forEach(function (version) {
                        if (currentVersion.SiteContentVersionID != version.SiteContentVersionID) {
                            version.Affiliates.forEach(function (affiliate) {
                                affiliates.push(affiliate.AffiliateID);
                            });
                        }
                    });

                    return affiliates;
                },
                currentVersion: function () {
                    return currentVersion;
                }
            }
        });

        modalInstance.result.then(function(version) {
            deferred.resolve(version);
        });

        return deferred.promise;
    };

    $scope.deleteSelectedVersion = function () {
        var version = $scope.options.Version;

        if (!confirm("Are you sure you want to delete the " + version.VersionName + " version?  This action cannot be undone."))
            return;

        contentService.deleteContentVersion(version).then(function () {
            var index = -1;

            $scope.content.Versions.forEach(function(item, idx) {
                if (item.SiteContentVersionID == version.SiteContentVersionID)
                    index = idx;
            });

            if (index != -1) {
                $scope.content.Versions.splice(index, 1);
                $scope.options.Version = $scope.content.Versions[0];
            }
        });
    };

    $scope.changeTemplate = function () {
        var version = $scope.options.Version,
            templates = $scope.content.Site.SiteTemplates,
            template = _.find(templates, function (t) { return t.SiteTemplateID == version.SiteTemplateID; });

        if (version.ContentText && version.ContentText.length) {
            if (template.DefaultContent && template.DefaultContent.length && confirm("Do you want to overwrite the existing content with the default template content?"))
                version.ContentText = template.DefaultContent;
        } else {
            version.ContentText = template.DefaultContent;
        }
    };

    //
    // Initialize
    //
    userService.getProfileTypes().then(function (data) {
        $scope.profileTypes = data;
    });

    affiliateService.getAffiliates().then(function (data) {
        $scope.affiliates = data;
    });

    if (!$scope.isNew()) {
        contentService.getContent($scope.content.SiteContentID).then(function(content) {
            $scope.updateContent(content);

            if (content.SiteContentTypeID == 1)
                $scope.activateContentTab = true;
        });
    } else {
        contentService.getSites().then(function (data) {
            $scope.sites = data;
            $scope.setSite();
            $scope.title = 'New Page';
        });
    }

}]);