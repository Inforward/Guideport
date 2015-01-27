'use strict';

angular.module('admin.Services').factory('geoService',
    ['dataService', 'cacheService', '$q', function (dataService, cacheService, $q) {

    return {

        getCountries: function () {
            var deferred = $q.defer();

            var params = {
                method: "get",
                url: "/api/geo/countries"
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

        getStateProvinces: function (countryCode) {
            var deferred = $q.defer();

            var params = {
                method: "get",
                url: "/api/geo/countries/" + countryCode + "/state-provinces"
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

