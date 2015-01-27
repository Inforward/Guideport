'use strict';

angular.module('admin.Directives').directive('pageSidebar', function () {
    return {
        restrict: 'A',
        replace: false,
        scope: true,
        link: function (scope, element, attrs) {

            element.addClass("page-sidebar");

            element.find(".close-btn").on("click", function() {
                element.removeClass("open").addClass("closed");
            });

            element.find(".open-btn").on("click", function () {
                element.removeClass("closed").addClass("open");
            });
        }
    }
});