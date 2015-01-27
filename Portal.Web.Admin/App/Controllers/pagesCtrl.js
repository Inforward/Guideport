'use strict';

angular.module('admin.Controllers')
    .controller("pagesCtrl", ['$scope', '$timeout', 'contentService', 'cacheService', function ($scope, $timeout, contentService, cacheService) {

    var filterKey = "content.pages.filters",
        filters = cacheService.getCache(filterKey);

    $scope.recordCount = -1;

    $scope.search = function () {
        cacheService.setCache(filterKey, $scope.filters);

        $scope.grid.dataSource.read();
    }

    $scope.resetFilters = function () {
        $scope.filters = {
            SiteID: 2,
            SearchText: null,
            SiteContentStatusID: 0
        };
    }

    if (filters) {
        $scope.filters = filters;
    } else {
        $scope.resetFilters();
    }

    contentService.getSites().then(function (sites) {
        $scope.sites = sites;

        $scope.gridOptions = {
            dataSource: {
                schema: {
                    model: {
                        id: 'SiteContentID',
                        fields: {
                            parentId: { field: "SiteContentParentID", nullable: true },
                            SiteContentID: { field: "SiteContentID", type: "number" }
                        },
                        expanded: true
                    }
                },
                batch: false,
                transport: {
                    read: function (e) {
                        contentService.getPages($scope.filters).then(function (data) {
                            $scope.recordCount = data.length;
                            $timeout(function () { e.success(data); }, 200);
                        });
                    },
                    destroy: function (e) {
                        contentService.trashContent(e.data.SiteContentID).then(e.success, e.error);
                    }
                }
            },
            remove: function (e) {
                if (!confirm('Are you sure you want to delete this page?'))
                    e.preventDefault();
            },
            autoBind: true,
            editable: true,
            pageable: false,
            sortable: true,
            height: 500,
            columns: [
                { field: 'Title', title: 'Title', width: 400, template: "<a title='Edit' class='file-link #=ContentTypeName.toLowerCase()# #if(!parentId){# top #}#' href='\\#/content/#=SiteID#/page/#=SiteContentID#\'>#=Title#</a>" },
                { field: 'StatusDescription', title: 'Status', width: 150, template: "<span class='#=StatusDescription.toLowerCase()#'>#=StatusDescription#</span>" },
                { field: 'ContentTypeName', title: 'Type', width: 150 },
                { field: 'VersionCount', title: 'No. of Versions', width: 150 },
                { field: 'ModifyDateUtc', title: 'Modified', template: "#= kendo.toString(kendo.parseDate(ModifyDateUtc),'MM/dd/yyyy hh:mm tt') #", width: 180 },
                { field: 'Permalink', title: ' ', width: 50, template: "<a class='k-button k-button-icontext btn-copy' ui-zeroclip zeroclip-text='#=Permalink#'></a>" },
                { title: ' ', width: 50, template: "<button data-command='destroy' class='k-button k-button-icontext btn-trash'><span class='k-icon k-delete'></span></button>" }]
        };
    });
}]);