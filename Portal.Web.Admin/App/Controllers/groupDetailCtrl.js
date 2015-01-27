'use strict';


angular.module('admin.Controllers')
    .controller("groupDetailCtrl", ['$scope', '$routeParams', '$modal', 'groupService', 'userService', function ($scope, $routeParams, $modal, groupService, userService) {

    $scope.groupId = $routeParams.id;
    $scope.model = groupService.defaultGroupModel();
    $scope.groupName = "New Group";
    $scope.isSaving = null;

    function updateModel(group) {
        $scope.model = group;
        $scope.groupName = group.Name;
        $scope.groupId = group.GroupID;
        $scope.isReadOnly = group.IsReadOnly;
    }

    if ($scope.groupId)
        groupService.getGroup($scope.groupId).then(updateModel);

    $scope.saveGroup = function (group) {
        $scope.isSaving = true;

        var successCallback = function(data) {
            updateModel(data);
            $scope.isSaving = false;
        }

        var errorCallback = function () {
            $scope.isSaving = null;
        }

        if (group.GroupID <= 0) {
            groupService.createGroup(group).then(successCallback, errorCallback);
        } else {
            groupService.updateGroup(group).then(successCallback, errorCallback);
        }
    }

}]);


angular.module('admin.Controllers')
    .controller("groupMemberUsersCtrl", ['$scope', '$modal', '$timeout', 'groupService', function ($scope, $modal, $timeout, groupService) {

        $scope.recordCount = -1;
        $scope.selectedMembers = [];

        $scope.addUsers = function () {
            var modalInstance = $modal.open({
                templateUrl: '/app/views/users/user-search-modal.html',
                controller: 'userSearchCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    groupId: function () {
                        return $scope.groupId;
                    }
                }
            });

            modalInstance.result.then(function(userIds) {
                groupService.addUserMembers($scope.groupId, userIds).then(function() {
                    dataSource.page(1);
                });
            });
        }

        $scope.removeUsers = function () {
            if (!$scope.selectedMembers.length)
                return;

            if (!confirm("Are you sure you want to remove these users?"))
                return;

            groupService.deleteUserMembers($scope.groupId, $scope.selectedMembers).then(function () {
                dataSource.read();
            });
        }

        var dataSource = new kendo.data.DataSource({
            serverPaging: true,
            pageSize: 250,
            schema: {
                model: { id: 'UserID' },
                data: "Results",
                total: "TotalRecordCount"
            },
            transport: {
                read: function (e) {
                    groupService.getUserMembers($scope.groupId, e.data).then(function (data) {
                        $scope.recordCount = data.TotalRecordCount;
                        $scope.selectedMembers = [];
                        $timeout(function () { e.success(data); }, 300);
                    });
                },
                destroy: function (e) {
                    groupService.deleteUserMembers($scope.groupId, [e.data.UserID]).then(function() {
                        e.success();
                        $scope.recordCount--;
                    });
                }
            }
        });

        $scope.populate = function () {
            dataSource.fetch();
        }

        $scope.$watch('isReadOnly', function (newValue, oldValue) {

            if (newValue == null)
                return;

            var columns = [
                { 'field': 'DisplayName', title: 'Name' },
                { 'field': 'ProfileTypeName', title: 'Profile Type', width: 200 },
                { 'field': 'Location', title: 'Location', width: 200 },
                { 'field': 'AffiliateName', title: 'Affiliate', width: 250 }];

            if (!$scope.isReadOnly)
                columns.push({ command: { name: 'destroy', text: ' ', className: 'btn-trash' }, title: ' ', width: 50 });

            $scope.gridOptions = {
                autoBind: false,
                readOnly: $scope.isReadOnly,
                sortable: false,
                pageable: {
                    refresh: false,
                    pageSizes: false,
                    buttonCount: 5
                },
                dataSource: dataSource,
                editable: 'popup',
                height: 500,
                columns: columns
            };
        });

    }]);


angular.module('admin.Controllers')
    .controller("groupMemberGroupsCtrl", ['$scope', '$modal', '$timeout', 'groupService', function ($scope, $modal, $timeout, groupService) {

        $scope.recordCount = -1;

        $scope.addGroups = function () {
            var modalInstance = $modal.open({
                templateUrl: '/app/views/groups/group-search-modal.html',
                controller: 'groupSearchCtrl',
                size: 'lg',
                resolve: {
                    groupId: function () {
                        return $scope.groupId;
                    }
                }
            });

            modalInstance.result.then(function (memberGroupIds) {
                groupService.addMemberGroups($scope.groupId, memberGroupIds).then(function () {
                    dataSource.read();
                });
            });
        }

        var dataSource = new kendo.data.DataSource({
            schema: {
                model: { id: 'GroupID' }
            },
            transport: {
                read: function (e) {
                    groupService.getMemberGroups($scope.groupId).then(function (data) {
                        $scope.recordCount = data.length;
                        $timeout(function () { e.success(data); }, 300);
                        e.success(data);
                    });
                },
                destroy: function (e) {
                    groupService.deleteMemberGroup($scope.groupId, e.data.GroupID).then(function() {
                        e.success();
                        $scope.recordCount--;
                    });
                }
            }
        });

        $scope.populate = function () {
            dataSource.fetch();
        }


        $scope.$watch('isReadOnly', function (newValue, oldValue) {

            if (newValue == null)
                return;

            var columns = [
                { 'field': 'Name', title: 'Group Name' },
                { 'field': 'Description' },
                { 'field': 'AccessibleUserCount', title: 'No. of Accessible Users', width: 250 },
                { 'field': 'MemberUserCount', title: 'No. of Users', width: 125 },
                { 'field': 'MemberGroupCount', title: 'No. of Groups', width: 125 }];

            if (!$scope.isReadOnly)
                columns.push({ command: { name: 'destroy', text: ' ', className: 'btn-trash' }, title: ' ', width: 50 });

            $scope.gridOptions = {
                autoBind: false,
                sortable: false,
                pageable: false,
                dataSource: dataSource,
                editable: 'popup',
                height: 500,
                columns: columns
            }
        });

    }]);

angular.module('admin.Controllers')
    .controller("groupAccessibilityCtrl", ['$scope', '$modal', '$timeout', 'groupService', function ($scope, $modal, $timeout, groupService) {

        $scope.recordCount = -1;

        $scope.addUsers = function() {
            var modalInstance = $modal.open({
                templateUrl: '/app/views/users/user-search-modal.html',
                controller: 'userSearchCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    groupId: function() {
                        return $scope.groupId;
                    }
                }
            });

            modalInstance.result.then(function(userIds) {
                groupService.addAccessibleUsers($scope.groupId, userIds).then(function() {
                    dataSource.page(1);
                });
            });
        };

        var dataSource = new kendo.data.DataSource({
            serverPaging: true,
            pageSize: 50,
            schema: {
                model: { id: 'UserID' }
            },
            transport: {
                read: function (e) {
                    groupService.getAccessibleUsers($scope.groupId, e.data).then(function (data) {
                        $scope.recordCount = data.length;
                        $timeout(function () { e.success(data); }, 300);
                    });
                },
                destroy: function (e) {
                    groupService.deleteAccessibleUsers($scope.groupId, [e.data.UserID]).then(function () {
                        e.success();
                        $scope.recordCount--;
                    });
                }
            }
        });

        $scope.populate = function () {
            dataSource.fetch();
        }

        $scope.gridOptions = {
            autoBind: false,
            sortable: false,
            pageable: false,
            dataSource: dataSource,
            editable: 'popup',
            height: 500,
            columns: [
                { 'field': 'DisplayName', title: 'Name' },
                { 'field': 'ProfileTypeName', title: 'Profile Type', width: 200 },
                { 'field': 'Location', title: 'Location', width: 200 },
                { 'field': 'AffiliateName', title: 'Affiliate', width: 250 },
                { 'field': 'UserID', title: ' ', template: "#if(!IsReadOnly){# <a class='k-button k-button-icontext btn-trash k-grid-delete' href='\\#'><span class='k-icon k-delete'></span></a> #}#", width: 50 }]
        };

    }]);