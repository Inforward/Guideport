'use strict';

angular.module('admin.Controllers').controller("userDetailCtrl",
    ['$scope', '$routeParams', '$timeout', 'userService', function ($scope, $routeParams, $timeout, userService) {

        $scope.userId = $routeParams.id;
        $scope.isSaving = null;

        userService.getUser($scope.userId).then(function(user) {
            $scope.user = user;

            if(user.Affiliate.Logos.length)
                $scope.logo = user.Affiliate.Logos[0];
        });

        $scope.populateRoles = function() {
            if (!$scope.roles) {
                userService.getUserRoles($scope.userId).then(function (roles) {
                    $scope.roles = roles;
                });
            }
        }

        $scope.saveRoles = function (form) {
            $scope.isSaving = true;

            userService.updateUserRoles($scope.userId, $scope.roles).then(function() {
                $scope.isSaving = false;
                form.$setPristine();
            });
        }

    }]);