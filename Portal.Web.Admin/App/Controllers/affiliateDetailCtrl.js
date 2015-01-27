'use strict';

angular.module('admin.Controllers').controller("affiliateDetailCtrl",
    ['$scope', '$routeParams', 'affiliateService', 'geoService', 'configurationService', function ($scope, $routeParams, affiliateService, geoService, configurationService) {

        $scope.affiliateId = $routeParams.id;
        $scope.isSaving = null;

        function updateStateProvinces(countryCode) {
            $scope.stateProvinces = geoService.getStateProvinces(countryCode).then(function(stateProvinces) {
                $scope.stateProvinces = stateProvinces;
            });                
        }

        function updatePostalCodeRegEx() {
            if ($scope.affiliate.Country) {
                var country = _.find($scope.countries, function (c) { return c.CountryCode == $scope.affiliate.Country; });

                if (country)
                    $scope.postalCodeRegEx = country.PostalCodeRegEx;
            }
        }

        function validateConfig(configuration) {

            return configurationService.validateConfiguration(configuration.ConfigurationID, configuration).then(function (errors) {

                var errorMessage = "";
                for (var environment in errors) {
                    if (errors.hasOwnProperty(environment) && errors[environment].length) {
                        errorMessage += "\n   " + environment + "\n      - " + errors[environment].join("\n      - ");
                    }
                }

                if (errorMessage != "") {
                    errorMessage = "Configuration errors found:\n" + errorMessage;
                    alert(errorMessage);
                    return false;
                }

                return true;
            });
        }

        geoService.getCountries().then(function (countries) {
            $scope.countries = countries;
        });

        affiliateService.getAffiliate($scope.affiliateId).then(function (affiliate) {
            $scope.affiliate = affiliate;
            $scope.name = affiliate.Name;

            $scope.affiliate.Logos.forEach(function (logo) {
                logo.uploader = { visible: (logo.FileID == 0) };

                logo.uploadOptions = {
                    async: { saveUrl: '/api/affiliates/' + affiliate.AffiliateID + '/logo/' + logo.AffiliateLogoTypeID, autoUpload: true },
                    localization: { select: 'Select file...', uploadSelectedFiles: 'Upload File' },
                    enabled: true,
                    multiple: false,
                    success: function (e) {
                        logo.FileID = e.response.FileID;
                        logo.FileInfo = e.response.FileInfo;
                        logo.uploader.visible = false;
                        $scope.$digest();

                        $(".k-upload-files").remove();
                        $(".k-upload-status").remove();
                    }
                };
            });

            if (affiliate.Logos.length)
                $scope.logo = _.find(affiliate.Logos, function(logo) { return logo.AffiliateLogoTypeID == 1; });

            if (affiliate.Country && affiliate.State)
                updateStateProvinces(affiliate.Country);

            updatePostalCodeRegEx();
        });

        $scope.saveAffiliate = function(form) {
            $scope.isSaving = true;

            affiliateService.updateAffiliate($scope.affiliate).then(function (data) {
                $scope.isSaving = false;
                $scope.name = $scope.affiliate.Name;
                form.$setPristine();
            });
        }

        $scope.changeCountry = function () {
            updatePostalCodeRegEx();
            updateStateProvinces($scope.affiliate.Country);
        }

        $scope.populateFeatures = function() {
            if (!$scope.features) {
                affiliateService.getFeatures($scope.affiliateId).then(function (features) {
                    $scope.features = features;
                });
            }
        }

        $scope.saveFeatures = function(form) {
            $scope.isSaving = true;

            affiliateService.updateFeatures($scope.affiliateId, $scope.features).then(function (features) {
                $scope.isSaving = false;
                $scope.features = features;
                form.$setPristine();
            });
        }

        $scope.isFeatureExpanded = function(feature) {
            if (!feature.Settings.length)
                return false;

            var disabledSettings = _.filter(feature.Settings, function(s) { return s.VisibleState == "Disabled"; }),
                enabledSettings = _.filter(feature.Settings, function (s) { return s.VisibleState == "Enabled"; });

            if (feature.IsEnabled && enabledSettings && enabledSettings.length)
                return true;

            return (!feature.IsEnabled && disabledSettings && disabledSettings.length);
        }

        $scope.populateSsoSettings = function () {
            if (!$scope.environments) {
                configurationService.getEnvironments().then(function(environments) {
                    $scope.environments = environments;
                });
            }

            if (!$scope.configuration) {
                configurationService.getConfigurations({
                    ConfigurationID: $scope.affiliate.SamlConfigurationID,
                    ConfigurationTypeName: "SAML.PartnerIdentityProvider"
                }).then(function (configurations) {
                    $scope.configuration = configurations[0];
                });
            }
        }

        $scope.saveSsoSettings = function (form) {

            var saveConfiguration = $.extend(true, {}, $scope.configuration);
            saveConfiguration.ConfigurationSettings = $.grep($scope.configuration.ConfigurationSettings, function (configSetting, j) {
                return configSetting.Value && configSetting.Value.length > 0;
            });

            validateConfig(saveConfiguration).then(function (isValid) {

                if (!isValid)
                    return;

                console.log("Saving...");
                $scope.isSaving = true;
                configurationService.saveConfiguration(saveConfiguration.ConfigurationID, saveConfiguration).then(function(data) {
                    $scope.saveAffiliate(form);
                    $scope.isSaving = false;
                    $scope.populateSsoSettings();
                });
            });
        }

        $scope.getEnvironmentSettings = function (environmentId) {
            var settings = [];

            if ($scope.configuration)
                settings = $.grep($scope.configuration.ConfigurationSettings, function (configSetting, j) {
                    return configSetting.EnvironmentID === environmentId;
                });

            return settings;
        }

        $scope.populateObjectives = function () {
            if (!$scope.objectives) {
                affiliateService.getObjectives($scope.affiliateId).then(function(objectives) {
                    $scope.objectives = objectives;
                });
            }
        }

        $scope.saveObjectives = function (form) {
            $scope.isSaving = true;

            affiliateService.updateObjectives($scope.affiliateId, $scope.objectives).then(function () {
                $scope.isSaving = false;
                form.$setPristine();
            });
        }
    }
]);
