'use strict';

angular.module('admin.Controllers')
    .controller("filesCtrl", ['$scope', '$timeout', 'contentService', 'cacheService', function ($scope, $timeout, contentService, cacheService) {

        var filterKey = "content.files.filters",
            filters = cacheService.getCache(filterKey);

        $scope.recordCount = -1;

        $scope.search = function () {
            cacheService.setCache(filterKey, $scope.filters);

            $scope.grid.dataSource.read();
        }

        $scope.resetFilters = function () {
            $scope.filters = {
                SiteID: 2,
                SearchText: null
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
                    schema: { model: { id: 'SiteContentID' } },
                    batch: false,
                    transport: {
                        read: function (e) {
                            contentService.getFiles($scope.filters).then(function (data) {
                                $scope.recordCount = data.length;
                                $timeout(function () { e.success(data); }, 200);
                            });
                        },
                        destroy: function (e) {
                            contentService.trashContent(e.data.SiteContentID).then(e.success, e.error);
                            $scope.recordCount--;
                        }
                    }
                },
                editable: 'popup',
                pageable: false,
                sortable: true,
                height: 500,
                columns: [
                    { field: 'Title', title: 'Title', template: "<a title='Edit' href='\\#/content/#=SiteID#/file/#=SiteContentID#\'>#=Title#</a>" },
                    { field: 'FileName', title: 'Filename' },
                    { field: 'FileType', title: 'File Type', width: 120 },
                    { field: 'FileSize', title: 'File Size', template: "#= kendo.toString(FileSize,'n0') + ' KB' #", width: 100 },
                    { field: 'ModifyDateUtc', title: 'Modified', template: "#= kendo.toString(kendo.parseDate(ModifyDateUtc),'MM/dd/yyyy hh:mm tt') #", width: 180 },
                    { field: 'KnowledgeLibrary', title: 'KL?', width: 60 },
                    { field: 'Permalink', title: ' ', width: 50, template: "<a class='k-button k-button-icontext btn-copy' ui-zeroclip zeroclip-text='#=Permalink#'></a>" },
                    { field: 'SiteContentID', title: ' ', template: "<a class='k-button k-button-icontext btn-trash k-grid-delete' href='\\#'><span class='k-icon k-delete'></span></a>", width: 50 }]
            };
        });
    }]);