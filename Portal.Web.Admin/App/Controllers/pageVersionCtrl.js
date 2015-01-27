'use strict';

angular.module('admin.Controllers').controller('pageVersionCtrl',
    ['$scope', '$modalInstance', 'affiliateService', 'existingAffiliates', 'currentVersion', function ($scope, $modalInstance, affiliateService, existingAffiliates, currentVersion) {

        $scope.title = (currentVersion.SiteContentVersionID > 0 ? 'Edit Version' : 'New Version');
        $scope.version = currentVersion;

        affiliateService.getAffiliates().then(function (data) {
            var affiliates = [];

            data.forEach(function (item) {
                if (existingAffiliates.indexOf(item.AffiliateID) == -1) {
                    affiliates.push(item);
                }
            });

            $scope.affiliates = affiliates;
        });

        $scope.affiliateOptions = {
            dataSource: {
                transport: {
                    read: function (e) {
                        affiliateService.getAffiliates().then(function(data) {
                            var affiliates = [];

                            data.forEach(function(item) {
                                if (existingAffiliates.indexOf(item.AffiliateID) == -1)
                                    affiliates.push(item);
                            });

                            e.success(affiliates);
                        });
                    }
                }
            },
            dataValueField: 'AffiliateID',
            dataTextField: 'Name'
        };

        $scope.isValid = function () {
            if ($scope.version == null)
                return false;

            return $scope.version.VersionName.length > 0 && $scope.version.Affiliates.length > 0;
        };

        $scope.ok = function () {
            $modalInstance.close($scope.version);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }]);