'use strict';

angular.module('admin.Controllers')
    .controller("thirdPartyResourceDetailCtrl", ['$scope', '$routeParams', 'contentService', 'geoService', 'affiliateService', function ($scope, $routeParams, contentService, geoService, affiliateService) {

        $scope.isSaving = null;

        $scope.resource = {
            ThirdPartyResourceID: $routeParams.id ? parseInt($routeParams.id) : null,
            Country: 'US'
        };

        $scope.isNew = function () {
            return $scope.resource.ThirdPartyResourceID == null;
        };

        $scope.updateResource = function (resource) {
            $scope.resource = resource;
            $scope.name = $scope.resource.Name;
            $scope.setStateProvinces();
        };

        $scope.servicesOptions = {
            dataSource: { transport: { read: function(e) { contentService.getThirdPartyServices().then(e.success); } } }
        };

        $scope.setStateProvinces = function() {
            var countryCode = $scope.resource.Country;

            if (countryCode && countryCode.length) {
                var country = _.find($scope.countries, function (c) { return c.CountryCode == countryCode; });

                if (country)
                    $scope.postalCodeRegEx = country.PostalCodeRegEx;

                geoService.getStateProvinces(countryCode).then(function (data) {
                    $scope.stateProvinces = data;
                });

            } else {
                $scope.stateProvinces = [];
                $scope.postalCodeRegEx = null;
            }
        };

        $scope.saveResource = function(form) {
            var resource = $scope.resource,
                method = $scope.isNew() ? contentService.createThirdPartyResource : contentService.updateThirdPartyResource;

            $scope.isSaving = true;

            resource.Services = resource.ServicesList.join(',');

            method(resource).then(function (data) {
                $scope.updateResource(data);
                $scope.isSaving = false;
                form.$setPristine();
            });

        };

        //
        // Initialize
        //
        geoService.getCountries().then(function (countries) {
            $scope.countries = countries;
        });

        affiliateService.getAffiliates().then(function (data) {
            $scope.affiliates = data;
        });

        if (!$scope.isNew()) {
            contentService.getThirdPartyResource($scope.resource.ThirdPartyResourceID).then(function(resource) {
                $scope.updateResource(resource);
            });
        } else {
            $scope.name = "New Third Party Resource";
            $scope.setStateProvinces();
        }

    }]);