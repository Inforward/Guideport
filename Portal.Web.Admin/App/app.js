'use strict';

angular.module('admin.Filters', []);
angular.module('admin.Services', []);
angular.module('admin.Controllers', ["admin.Services", "admin.Filters", "ui.bootstrap"]);
angular.module('admin.Directives', ["ui.bootstrap"]);

angular.module("adminApp", ["kendo.directives", "ui.bootstrap", "ui.bootstrap-switch", "ngRoute", "ngAnimate", "ngSanitize", "admin.Controllers", "admin.Services", "admin.Directives", "admin.Filters"])
       .config(['$routeProvider', '$tooltipProvider', function ($routeProvider, $tooltipProvider) {

        $routeProvider
            .when("/content/pages", { templateUrl: "/app/views/content/pages.html", controller: "pagesCtrl", title: 'Content - Pages', bodyClass: 'content' })
            .when("/content/:siteId/page/:id?", { templateUrl: "/app/views/content/page-detail.html", controller: "pageDetailCtrl", title: 'Content - Pages', bodyClass: 'content' })
            .when("/content/files", { templateUrl: "/app/views/content/files.html", controller: "filesCtrl", title: 'Content - Files', bodyClass: 'content' })
            .when("/content/:siteId/file/:id?", { templateUrl: "/app/views/content/file-detail.html", controller: "fileDetailCtrl", title: 'Content - Files', bodyClass: 'content' })
            .when("/content/third-party-resources", { templateUrl: "/app/views/content/third-party-resources.html", controller: "thirdPartyResourcesCtrl", title: 'Content - Third Party Resources', bodyClass: 'content' })
            .when("/content/third-party-resource/:id?", { templateUrl: "/app/views/content/third-party-resource-detail.html", controller: "thirdPartyResourceDetailCtrl", title: 'Content - Third Party Resources', bodyClass: 'content' })
            .when("/surveys/:surveyId", { templateUrl: "/app/views/surveys/survey.html", controller: "surveyCtrl", title: 'Survey', bodyClass: 'survey' })
            .when("/reports/:reportId", { templateUrl: "/app/views/reports/report.html", controller: "reportCtrl", title: 'Reporting', bodyClass: 'reporting' })
            .when("/groups", { templateUrl: "/app/views/groups/groups.html", controller: "groupsCtrl", title: 'Groups', bodyClass: 'groups' })
            .when("/groups/detail/:id?", { templateUrl: "/app/views/groups/group-detail.html", controller: "groupDetailCtrl", title: 'Groups', bodyClass: 'groups' })
            .when("/users", { templateUrl: "/app/views/users/users.html", controller: "usersCtrl", title: 'Users', bodyClass: 'users' })
            .when("/users/:id", { templateUrl: "/app/views/users/user-detail.html", controller: "userDetailCtrl", title: 'Users', bodyClass: 'users' })
            .when("/affiliates", { templateUrl: "/app/views/affiliates/affiliates.html", controller: "affiliatesCtrl", title: 'Affiliates', bodyClass: 'affiliates' })
            .when("/affiliates/detail/:id?", { templateUrl: "/app/views/affiliates/affiliate-detail.html", controller: "affiliateDetailCtrl", title: 'Affiliates', bodyClass: 'affiliates' })
            .when("/", { templateUrl: "/app/views/home.html", controller: "adminCtrl" })
            .otherwise({ redirectTo: "/" });

        $tooltipProvider.options({
            placement: 'top',
            animation: true,
            popupDelay: 0,
            appendToBody: true
        });

       }])

    .run(['$rootScope', function ($rootScope) {
        $rootScope.$on("$routeChangeSuccess", function (event, currentRoute, previousRoute) {
            var title = currentRoute.title || 'Home';

            $rootScope.title = 'Admin Console | ' + title;
            $rootScope.bodyClass = currentRoute.bodyClass;
        });
    }]);