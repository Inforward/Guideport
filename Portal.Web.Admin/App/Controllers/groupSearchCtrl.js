'use strict';

angular.module('admin.Controllers').controller('groupSearchCtrl',
    ['$scope', '$modalInstance', 'groupService', 'groupId', function ($scope, $modalInstance, groupService, groupId) {

        $scope.selectedGroups = [];

        $scope.filter = {
            IncludeMemberCounts: true,
            ExcludeGroupID: groupId
        };

        $scope.ok = function () {
            $modalInstance.close($scope.selectedGroups);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        $scope.gridOptions = {
            pageable: false,
            dataSource: {
                schema: {
                    model: { id: 'GroupID' }
                },
                transport: {
                    read: function (e) {
                        groupService.getGroups($scope.filter).then(e.success);
                    }
                }
            },
            height: 350,
            columns: [
                { 'field': 'Name', title: 'Group Name' },
                { 'field': 'AccessibleUserCount', title: 'No. of Accessible Users', width: 250 },
                { 'field': 'MemberUserCount', title: 'No. of Users', width: 125 },
                { 'field': 'MemberGroupCount', title: 'No. of Groups', width: 125 }]
        };
    }]);