'use strict';

angular.module('admin.Controllers').controller("usersCtrl",
    ['$scope', '$routeParams', '$timeout', 'userService', 'affiliateService', function ($scope, $routeParams, $timeout, userService, affiliateService) {

        $scope.isSearching = false;
        $scope.recordCount = -1;
        $scope.profileTypes = [];
        $scope.affiliates = [];
        $scope.statuses = [];
        $scope.grid = {};
        $scope.filters = {
            FirstName: null,
            LastName: null,
            ProfileTypeID: -1,
            AffiliateID: -1,
            UserStatusID: 1
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

        $scope.search = function() {
            if ($scope.grid.dataSource.page() != 1)
                $scope.grid.dataSource.page(1);
            else
                $scope.grid.dataSource.read();
        }

        $scope.resetFilters = function() {
            $scope.filters = {
                FirstName: null,
                LastName: null,
                ProfileTypeID: -1,
                AffiliateID: -1,
                UserStatusID: -1
            };
        }

        $scope.gridOptions = {
            autoBind: true,
            sortable: {
                mode: 'single',
                allowUnsort: false
            },
            pageable: {
                refresh: false,
                pageSizes: false,
                buttonCount: 5
            },
            height: 500,
            columns: [
                { field: 'DisplayName', title: 'Name', locked: true, lockable: false, width: 250, template: "<a href='\\#/users/#=UserID#\'>#=DisplayName#</a>" },
                { field: 'ProfileTypeName', title: 'Profile Type', width: 200 },
                { field: 'AffiliateName', title: 'Affiliate', width: 230 },
                { field: 'UserStatusName', title: 'Status', width: 150 },
                { field: 'BusinessConsultantDisplayName', title: 'Business Consultant', width: 200 },
                { field: 'PrimaryPhone', title: 'Phone No', width: 180 },
                { field: 'Email', title: 'Email', width: 280 },
                { field: 'ModifyDate', title: 'Modified', width: 200, template: "#= kendo.toString(kendo.parseDate(ModifyDate),'MM/dd/yyyy hh:mm tt') #" }],
            dataSource: {
                serverPaging: true,
                serverSorting: true,
                pageSize: 250,
                schema: {
                    data: "Results",
                    total: "TotalRecordCount"
                },
                transport: {
                    read: function (e) {
                        $scope.isSearching = true;

                        userService.getUsers($.extend({}, $scope.filters, e.data)).then(function (data) {
                            $scope.recordCount = data.TotalRecordCount;
                            $scope.isSearching = false;
                            $timeout(function () { e.success(data); }, 300);
                        });
                    }
                }
            }
        }

    }]);