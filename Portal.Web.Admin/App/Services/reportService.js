'use strict';

angular.module('admin.Services').factory('reportService', ['dataService', function (dataService) {

    return {

        getReportMetaData: function (reportId) {
            var params = {
                method: "get",
                url: "/api/reports/" + reportId
            };

            return dataService.send(params);
        },

        executeReport: function(request) {
            var params = {
                method: "post",
                url: "/api/reports",
                data: request
            };

            return dataService.send(params);
        }
    }
}]);

