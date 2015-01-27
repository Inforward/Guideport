'use strict';

angular.module('admin.Controllers')
    .controller("thirdPartyResourcesCtrl", ['$scope', '$timeout', 'contentService', 'cacheService', function ($scope, $timeout, contentService, cacheService) {

        $scope.recordCount = -1;

        $scope.search = function () {
            $scope.grid.dataSource.read();
        }

        $scope.gridOptions = {
            dataSource: {
                schema: { model: { id: 'ThirdPartyResourceID' } },
                batch: false,
                transport: {
                    read: function (e) {
                        contentService.getThirdPartyResources().then(function (data) {
                            $scope.recordCount = data.length;
                            $timeout(function () { e.success(data); }, 200);
                        });
                    },
                    destroy: function (e) {
                        contentService.deleteThirdPartyResource(e.data.ThirdPartyResourceID).then(e.success, e.error);
                        $scope.recordCount--;
                    }
                }
            },
            editable: 'popup',
            pageable: false,
            sortable: true,
            height: 500,
            columns: [
                { 'field': 'Name', template: "<a href='\\#/content/third-party-resource/#=ThirdPartyResourceID#'>#=Name#</a>" },
                { 'field': 'AddressHtml', title: 'Address', width: 200, encoded: false },
                { 'field': 'ServicesHtml', title: 'Services', width: 175, encoded: false },
                { 'field': 'Email', title: 'Email' },
                { 'field': 'PhoneNo', title: 'Phone' },
                { 'field': 'WebsiteUrl', title: 'Website' },
                { 'field': 'ModifyDate', title: 'Modified', template: '#= kendo.toString(kendo.parseDate(ModifyDateUtc), "MM/dd/yyyy hh:mm tt") #', width: 170 },
                { command: { name: 'destroy', text: ' ', className: 'btn-trash' }, title: ' ', width: 50 }]
        };
    }]);