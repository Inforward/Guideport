'use strict';

angular.module('admin.Directives').directive('navAccordion', function() {
    return {
        restrict: 'A',
        replace: false,
        scope: true,
        controller: ['$scope', function ($scope) {
            
        }],
        link: function(scope, element, attrs) {
            element.find("li.parent > a").on("click", function () {
                element.find("li.parent ul:visible").slideUp();

                var $submenu = $(this).next("ul");

                if ($submenu.is(":visible"))
                    $submenu.slideUp();
                else
                    $submenu.slideDown();
            });
        }
    }
});