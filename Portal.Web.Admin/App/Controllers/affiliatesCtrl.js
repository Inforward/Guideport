'use strict';

angular.module('admin.Controllers')
    .controller("affiliatesCtrl", ['$scope', '$timeout', 'affiliateService', function ($scope, $timeout, affiliateService) {

    $scope.recordCount = -1;

    $scope.gridOptions = {
        dataSource: {
            schema: {
                model: {
                    id: 'AffiliateID'
                }
            },
            batch: false,
            transport: {
                read: function (e) {

                    var request = {
                        IncludeUserCount: true,
                        UseCache: false
                    };

                    affiliateService.getAffiliates(request).then(function (data) {
                        $scope.recordCount = data.length;
                        $timeout(function() { e.success(data); }, 200);
                    });
                }
            }
        },
        pageable: false,
        height: 500,
        columns: [
            { 'field': 'Name', title: 'Affiliate Name', template: "<a href='\\#/affiliates/detail/#=AffiliateID#\'>#=Name#</a>", width: 200 },
            { 'field': 'ExternalID', title: 'External ID', width: 100 },
            { 'field': 'Phone', title: 'Phone', width: 130 },
            { 'field': 'WebsiteUrl', title: 'Website', width: 250 },
            { 'field': 'Address', title: 'Address', width: 220, encoded: false },
            { 'field': 'UserCount', title: 'No. of Users', dataFormat: '{0:N2}', width: 100 },
            { 'field': 'ModifyDateUtc', title: 'Modified', template: "#= kendo.toString(kendo.parseDate(ModifyDateUtc),'MM/dd/yyyy hh:mm tt') #", width: 170 }]
    };

}]);