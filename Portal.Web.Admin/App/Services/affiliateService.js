'use strict';

angular.module('admin.Services').factory('affiliateService',
    ['dataService', 'cacheService', '$q', function (dataService, cacheService, $q) {

    return {

        getAffiliates: function (request) {
            var deferred = $q.defer();

            var params = {
                method: "get",
                url: "/api/affiliates",
                params: request
            };

            var useCache = request == undefined || request.UseCache == undefined ? true : request.UseCache;
            var cache = cacheService.getSessionCache(params.url);

            if (useCache && cache)
                deferred.resolve(cache);
            else
                dataService.send(params).then(function (data) {
                    cacheService.setSessionCache(params.url, data);
                    deferred.resolve(data);
                });

            return deferred.promise;
        },

        getAffiliate: function (affiliateId) {
            var params = {
                method: "get",
                url: "/api/affiliates/" + affiliateId
            };

            return dataService.send(params);
        },

        getFeatures: function (affiliateId) {
            var params = {
                method: "get",
                url: "/api/affiliates/" + affiliateId + "/features"
            };

            return dataService.send(params);
        },

        getObjectives: function (affiliateId) {
            var params = {
                method: "get",
                url: "/api/affiliates/" + affiliateId + "/objectives"
            };

            return dataService.send(params);
        },

        updateFeatures: function(affiliateId, features) {
            var params = {
                method: "put",
                url: "/api/affiliates/" + affiliateId + "/features",
                data: features
            };

            return dataService.send(params);
        },

        updateAffiliate: function(affiliate) {
            var params = {
                method: "put",
                url: "/api/affiliates/" + affiliate.AffiliateID,
                data: affiliate
            };

            return dataService.send(params);
        },

        updateObjectives: function(affiliateId, objectives) {
            var params = {
                method: "put",
                url: "/api/affiliates/" + affiliateId + "/objectives",
                data: objectives
            };

            return dataService.send(params);
        }
    }
}]);

