'use strict';

angular.module('admin.Services').factory('groupService',
    ['dataService', 'cacheService', '$q', function (dataService, cacheService, $q) {

    return {

        defaultGroupModel: function() {
            return {
                GroupID: 0,
                GroupTypeID: 2,
                IsReadOnly: false,
                Owner: {
                    UserID: null,
                    DisplayName: null
                }
            }
        },

        getGroups: function (filter) {
            var params = {
                method: "get",
                url: "/api/groups",
                params: filter
            };

            return dataService.send(params);
        },

        getGroup: function (id) {
            var params = {
                method: "get",
                url: "/api/groups/" + id
            };

            return dataService.send(params);
        },

        createGroup: function(group) {
            var params = {
                method: "post",
                url: "/api/groups",
                data: group
            };

            return dataService.send(params);
        },

        updateGroup: function(group) {
            var params = {
                method: "put",
                url: "/api/groups/" + group.GroupID,
                data: group
            };

            return dataService.send(params);
        },

        removeGroup: function(groupId) {
            var params = {
                method: "delete",
                url: "/api/groups/" + groupId
            };

            return dataService.send(params);
        },

        getUserMembers: function (groupId, filters) {
            var params = {
                method: "get",
                url: "/api/groups/" + groupId + "/users",
                params: filters
            };

            return dataService.send(params);
        },

        addUserMembers: function(groupId, userIds) {
            var params = {
                method: "post",
                url: "/api/groups/" + groupId + "/users",
                data: userIds
            };

            return dataService.send(params);
        },

        deleteUserMembers: function(groupId, userIds) {
            var params = {
                method: "put",
                url: "/api/groups/" + groupId + "/users",
                data: userIds
            };

            return dataService.send(params);
        },

        getAccessibleUsers: function (groupId, filters) {
            var params = {
                method: "get",
                url: "/api/groups/" + groupId + "/accessible-users",
                params: filters
            };

            return dataService.send(params);
        },

        addAccessibleUsers: function (groupId, userIds) {
            var params = {
                method: "post",
                url: "/api/groups/" + groupId + "/accessible-users",
                data: userIds
            };

            return dataService.send(params);
        },

        deleteAccessibleUsers: function(groupId, userIds) {
            var params = {
                method: "put",
                url: "/api/groups/" + groupId + "/accessible-users",
                data: userIds
            };

            return dataService.send(params);
        },

        getMemberGroups: function(groupId) {
            var params = {
                method: "get",
                url: "/api/groups/" + groupId + "/groups"
            };

            return dataService.send(params);
        },

        addMemberGroups: function(groupId, memberGroupIds) {
            var params = {
                method: "post",
                url: "/api/groups/" + groupId + "/groups",
                data: memberGroupIds
            };

            return dataService.send(params);
        },
        
        deleteMemberGroup: function(groupId, memberGroupId) {
            var params = {
                method: "delete",
                url: "/api/groups/" + groupId + "/groups/" + memberGroupId
            };

            return dataService.send(params);
        }
    }
}]);