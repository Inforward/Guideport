'use strict';

angular.module('admin.Services').factory('configurationService',
    ['dataService', function (dataService) {

    return {

        getConfigurations: function (request) {
            var params = {
                method: "get",
                url: "/api/app/configurations",
                params: request
            };

            return dataService.send(params);
        },

        getSettings: function (request) {
            var params = {
                method: "get",
                url: "/api/app/settings/",
                params: request
            };

            return dataService.send(params);
        },

        getEnvironments: function () {
            var params = {
                method: "get",
                url: "/api/app/environments"
            };

            return dataService.send(params);
        },

        validateConfiguration: function(id, configuration) {
            var params = {
                method: "put",
                url: "/api/app/configurations/" + id + "/validate/",
                data: configuration
            };

            return dataService.send(params);
        },

        saveConfiguration: function(id, configuration) {
            var params = {
                method: "put",
                url: "/api/app/configurations/" + id,
                data: configuration
            };

            return dataService.send(params);
        }
    }
}]);

