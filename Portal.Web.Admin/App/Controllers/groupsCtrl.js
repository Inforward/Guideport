'use strict';

angular.module('admin.Controllers')
    .controller("groupsCtrl", ['$scope', '$timeout', 'groupService', function ($scope, $timeout, groupService) {

    $scope.recordCount = -1;

    $scope.model = {
        searchText: null
    };

    $scope.gridOptions = {
        dataSource: {
            schema: {
                model: {
                    id: 'GroupID'
                }
            },
            batch: false,
            transport: {
                read: function (e) {

                    var request = {
                        IncludeMemberCounts: true
                    };

                    groupService.getGroups(request).then(function (data) {
                        $scope.recordCount = data.length;
                        $timeout(function() { e.success(data); }, 200);
                    });
                },
                destroy: function (e) {
                    groupService.removeGroup(e.data.GroupID).then(e.success);
                }
            }
        },
        editable: 'popup',
        pageable: false,
        height: 500,
        columns: [
            { 'field': 'Name', title: 'Group Name', template: "<a href='\\#/groups/detail/#=GroupID#\'>#=Name#</a>" },
            { 'field': 'Description' },
            { 'field': 'AccessibleUserCount', title: 'No. of Accessible Users', width: 225 },
            { 'field': 'MemberUserCount', title: 'No. of Users', width: 125 },
            { 'field': 'MemberGroupCount', title: 'No. of Groups', width: 125 },
            { 'field': 'ModifyDateUtc', title: 'Modified', template: "#= kendo.toString(kendo.parseDate(ModifyDateUtc),'MM/dd/yyyy hh:mm tt') #", width: 170 },
            { 'field': 'GroupID', title: ' ', template: "#if(!IsReadOnly){# <a class='k-button k-button-icontext btn-trash k-grid-delete' href='\\#'><span class='k-icon k-delete'></span></a> #}#", width: 50 }]
    };

}]);