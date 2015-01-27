'use strict';

angular.module('admin.Directives').directive('confirmOnExit', function () {
    return {
        restrict: "A",
        link: function ($scope, elem, attrs) {

            var formName = elem.attr("name"),
                form = $scope[formName];

            var $handler = function() {
                if (form.$dirty)
                    return "You have unsaved changes.";
            };

            // Attach handler to catch any browser url changes/closings
            $(window).on('beforeunload', $handler);

            // Also attach to router event to ensure any angular view changes are caught as well
            $scope.$on('$locationChangeStart', function (event, next, current) {
                if (form.$dirty && !confirm("You have unsaved changes.\n\nAre you sure you want to leave this page?"))
                    event.preventDefault();
            });

            // If we navigate away via angular, ensure we remove the handler on the window
            $scope.$on('$locationChangeSuccess', function(event, next, current) {
                $(window).off('beforeunload', $handler);
            });
        }
    };
});