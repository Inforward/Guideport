'use strict';

var adminServices = angular.module('admin.Services', []);

angular.module('admin.Services')
    .factory('dataService', ['$http', '$q', function ($http, $q) {

    return {
        send: function (options) {
            return $http(options).then(function (response) {

                return response.data;

            }, function (response) {

                if (response.data && response.data.ExceptionMessage) {
                    alert(response.data.ExceptionMessage);
                    return $q.reject(response.data.ExceptionMessage);
                }
                
                if (response.data && response.data.Message) {
                    alert(response.data.Message);
                    return $q.reject(response.data.Message);
                }

                return $q.reject("An unknown error occurred.");
            });
        }
    }

}]);