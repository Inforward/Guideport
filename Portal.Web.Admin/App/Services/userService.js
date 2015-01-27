'use strict';

angular.module('admin.Services').factory('userService', ['dataService', 'cacheService', '$q', function (dataService, cacheService, $q) {

    return {

        getUsers: function (request) {

            // CH: using jQuery to serialize the parameters since angular won't
            // do this correctly with nested complex arrays (like the sort parameters)
            // Annoying since we're introducing a dependency that is not injected

            var params = {
                method: "get",
                url: "/api/users/?" + $.param(request)
            };

            return dataService.send(params);
        },

        getUser: function(userId) {
            var params = {
                method: "get",
                url: "/api/users/" + userId
            };

            return dataService.send(params);
        },

        getUserRoles: function(userId) {
            var params = {
                method: "get",
                url: "/api/users/" + userId + "/roles"
            };

            return dataService.send(params);
        },

        updateUserRoles: function(userId, roles) {
            var params = {
                method: "put",
                url: "/api/users/" + userId + "/roles",
                data: roles
            };

            return dataService.send(params);
        },

        getProfileTypes: function () {
            var deferred = $q.defer();

            var params = {
                method: "get",
                url: "/api/users/profile-types"
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

        getUserStatuses: function () {
            var deferred = $q.defer();

            var params = {
                method: "get",
                url: "/api/users/statuses"
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

