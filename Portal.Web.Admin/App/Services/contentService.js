'use strict';

angular.module('admin.Services').factory('contentService', ['dataService', 'cacheService', '$q', function (dataService, cacheService, $q) {

    return {

        getPages: function (filters) {
            var params = {
                method: "get",
                url: "/api/contents/pages",
                params: filters
            };

            return dataService.send(params);
        },

        getParents: function (siteId, excludeId) {
            var params = {
                method: "get",
                url: "/api/contents/pages/" + siteId + "/parents"
            };

            if (excludeId)
                params.url += "/" + excludeId;

            return dataService.send(params);
        },

        getFiles: function (filters) {
            var params = {
                method: "get",
                url: "/api/contents/files",
                params: filters
            };

            return dataService.send(params);
        },

        getSites: function () {
            var deferred = $q.defer();

            var params = {
                method: "get",
                url: "/api/contents/sites"
            };

            var cache = cacheService.getSessionCache(params.url);

            if (cache)
                deferred.resolve(cache);
            else
                dataService.send(params).then(function(data) {
                    cacheService.setSessionCache(params.url, data);
                    deferred.resolve(data);
                });

            return deferred.promise;
        },

        getMenuIcons: function () {
            var deferred = $q.defer();

            var params = {
                method: "get",
                url: "/api/contents/menu-icons"
            };

            var cache = cacheService.getSessionCache(params.url);

            if (cache)
                deferred.resolve(cache);
            else
                dataService.send(params).then(function(data) {
                    cacheService.setSessionCache(params.url, data);
                    deferred.resolve(data);
                });

            return deferred.promise;
        },

        trashContent: function(id) {
            var params = {
                method: "delete",
                url: "/api/contents/" + id
            };

            return dataService.send(params);
        },

        getContent: function (contentId) {
            var params = {
                method: "get",
                url: "/api/contents/" + contentId
            };

            return dataService.send(params);
        },

        saveContent: function(content) {
            if (content.SiteContentID > 0)
                return this.updateContent(content);

            return this.createContent(content);
        },

        createContent: function(content) {
            var params = {
                method: "post",
                url: "/api/contents",
                data: content
            };

            return dataService.send(params);
        },

        updateContent: function(content) {
            var params = {
                method: "put",
                url: "/api/contents/" + content.SiteContentID,
                data: content
            };

            return dataService.send(params);
        },

        generatePermalink: function(content) {
            var params = {
                method: "get",
                url: "/api/contents/permalinks",
                params: {
                    SiteID: content.SiteID,
                    SiteContentParentID: content.SiteContentParentID,
                    SiteContentTypeID: content.SiteContentTypeID,
                    Title: content.Title
                }
            };

            return dataService.send(params);
        },

        validatePermalink: function (permalink, siteId, contentId) {
            var params = {
                method: "put",
                url: "/api/contents/permalinks",
                data: {
                    SiteID: siteId,
                    Permalink: permalink,
                    SiteContentID: contentId
                }
            };

            return dataService.send(params);
        },

        createContentVersion: function(version) {
            var params = {
                method: "post",
                url: "/api/contents/" + version.SiteContentID + "/versions",
                data: version
            };

            return dataService.send(params);
        },

        updateContentVersion: function(version) {
            var params = {
                method: "put",
                url: "/api/contents/" + version.SiteContentID + "/versions/" + version.SiteContentVersionID,
                data: version
            };

            return dataService.send(params);
        },

        deleteContentVersion: function(version) {
            var params = {
                method: "delete",
                url: "/api/contents/" + version.SiteContentID + "/versions/" + version.SiteContentVersionID
            };

            return dataService.send(params);
        },

        getTopics: function (siteId) {
            var deferred = $q.defer();

            var params = {
                method: "get",
                url: "/api/contents/sites/" + siteId + "/topics"
            };

            var cache = cacheService.getSessionCache(params.url);

            if (cache)
                deferred.resolve(cache);
            else
                dataService.send(params).then(function (data) {
                    cacheService.setSessionCache(params.url, data);
                    deferred.resolve(data);
                });

            return deferred.promise;
        },

        getThirdPartyResources: function() {
            var params = {
                method: "get",
                url: "/api/contents/third-party-resources"
            };

            return dataService.send(params);
        },

        getThirdPartyResource: function(id) {
            var params = {
                method: "get",
                url: "/api/contents/third-party-resources/" + id
            };

            return dataService.send(params);
        },

        deleteThirdPartyResource: function(id) {
            var params = {
                method: "delete",
                url: "/api/contents/third-party-resources/" + id
            };

            return dataService.send(params);
        },

        createThirdPartyResource: function(resource) {
            var params = {
                method: "post",
                url: "/api/contents/third-party-resources",
                data: resource
            };

            return dataService.send(params);
        },

        updateThirdPartyResource: function(resource) {
            var params = {
                method: "put",
                url: "/api/contents/third-party-resources/" + resource.ThirdPartyResourceID,
                data: resource
            };

            return dataService.send(params);
        },

        getThirdPartyServices: function() {
            var deferred = $q.defer();

            var params = {
                method: "get",
                url: "/api/contents/third-party-resources/services"
            };

            var cache = cacheService.getSessionCache(params.url);

            if (cache)
                deferred.resolve(cache);
            else
                dataService.send(params).then(function (data) {
                    cacheService.setSessionCache(params.url, data);
                    deferred.resolve(data);
                });

            return deferred.promise;
        }
    }
}]);