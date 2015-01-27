'use strict';

angular.module('admin.Controllers').controller('userSearchCtrl',
    ['$scope', '$modalInstance', '$timeout', 'userService', 'affiliateService', 'groupId', function ($scope, $modalInstance, $timeout, userService, affiliateService, groupId) {

    $scope.selectedUsers = [];
    $scope.recordCount = -1;
    $scope.profileTypes = [];
    $scope.affiliates = [];
    $scope.statuses = [];

    $scope.filter = {
        Name: null,
        ProfileTypeID: -1,
        AffiliateID: -1,
        UserStatusID: -1,
        ExcludeMemberGroupID: groupId,
        IncludeAffiliates: true
    };

    affiliateService.getAffiliates().then(function (data) {
        data.unshift({ 'AffiliateID': -1, 'Name': 'All Affiliates' });
        $scope.affiliates = data;
    });

    userService.getProfileTypes().then(function (data) {
        data.unshift({ 'ProfileTypeID': -1, 'Name': 'All Profile Types' });
        $scope.profileTypes = data;
    });

    userService.getUserStatuses().then(function (data) {
        data.unshift({ 'UserStatusID': -1, 'Name': 'All Statuses' });
        $scope.statuses = data;
    });

    var gridDataSource = new kendo.data.DataSource({
        serverPaging: true,
        serverSorting: true,
        pageSize: 250,
        schema: {
            model: { id: 'UserID' },
            data: "Results",
            total: "TotalRecordCount"
        },
        transport: {
            read: function (e) {
                $scope.isSearching = true;

                userService.getUsers($.extend({}, $scope.filter, e.data)).then(function (data) {
                    $scope.recordCount = data.TotalRecordCount;
                    $scope.isSearching = false;
                    $timeout(function() { e.success(data); }, 300);
                });
            }
        }
    });

    $scope.resetFilters = function () {
        $scope.filter = angular.extend({}, $scope.filter, {
            Name: null,
            ProfileTypeID: -1,
            AffiliateID: -1,
            UserStatusID: -1
        });
    }

    $scope.search = function () {
        $scope.selectedUsers = [];

        if (gridDataSource.page() == 1)
            gridDataSource.read();
        else
            gridDataSource.page(1);
    }

    $scope.ok = function () {
        $modalInstance.close($scope.selectedUsers);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.gridOptions = {
        sortable: {
            mode: 'single',
            allowUnsort: false
        },
        autoBind: false,
        readOnly: false,
        pageable: {
            refresh: false,
            pageSizes: false,
            buttonCount: 5
        },
        dataSource: gridDataSource,
        height: 350,
        columns: [
            { field: 'DisplayName', title: 'Name' },
            { field: 'ProfileTypeName', title: 'Profile Type', width: 200 },
            { field: 'AffiliateName', title: 'Affiliate' },
            { field: 'UserStatusName', title: 'Status', width: 150 }, ]
    };
}]);