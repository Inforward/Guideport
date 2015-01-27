'use strict';

angular.module('admin.Services').factory('surveyService', ['dataService', function (dataService) {

    return {

        getSurvey: function(surveyId) {
            var params = {
                method: "get",
                url: "/api/surveys/" + surveyId
            };

            return dataService.send(params);
        },

        updateSurvey: function (survey) {
            var params = {
                method: "put",
                url: "/api/surveys/" + survey.SurveyID,
                data: survey
            };

            return dataService.send(params);
        },
    }
}]);

