'use strict';

angular.module('admin.Directives').directive('processing', function () {
    return {
        restrict: 'A',
        scope: {
            message: '@',
            completeMessage: '@',
            inProgress: '='
        },
        controller: ['$scope', '$element', '$timeout', function ($scope, $element, $timeout) {

            $scope.$watch('inProgress', function (newValue, oldValue) {

                if (newValue === true) {
                    $element.empty().append("<span class='indicator'>" + $scope.message + "</span>").show();

                } else if (newValue === false) {

                    if ($scope.completeMessage && $scope.completeMessage.length) {
                        $element.empty().append("<span>" + $scope.completeMessage + "</span>");
                        $timeout(function() { $element.fadeOut(); }, 3000);
                    } else {
                        $element.hide();
                    }
                } else {
                    $element.hide();
                }
            });
        }],
        link: function(scope, element, attrs) {
            element.addClass("processing");
        }
    }
});