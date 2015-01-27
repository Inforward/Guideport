'use strict';

angular.module('admin.Controllers')
    .controller("surveyCtrl", ['$scope', '$routeParams', 'surveyService', function($scope, $routeParams, surveyService) {

    $scope.surveyId = $routeParams.surveyId;
    $scope.isSaving = null;
    $scope.suggestedContents = [];

    surveyService.getSurvey($scope.surveyId).then(function (response) {

        $scope.survey = response.Survey;
        $scope.suggestedContents = response.SuggestedContents;

    });

    $scope.saveSurvey = function(survey) {
        $scope.isSaving = true;

        surveyService.updateSurvey(survey).then(function() {
            $scope.isSaving = false;
        });
    }

    $scope.suggestedContentSelectOptions = {
        placeholder: "Select contents...",
        dataTextField: "TitlePath",
        dataValueField: "SiteContentID",
        autoBind: false,
        dataSource: {
            transport: {
                read: function (e) {
                    e.success($scope.suggestedContents);
                }
            }
        }
    };
}]);
