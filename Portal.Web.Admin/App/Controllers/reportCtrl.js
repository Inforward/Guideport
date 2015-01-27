'use strict';

angular.module('admin.Controllers').controller("reportCtrl",
    ['$scope', '$routeParams', '$timeout', 'reportService', function ($scope, $routeParams, $timeout, reportService) {

        $scope.reportId = $routeParams.reportId;
        $scope.report = null;
        $scope.grid = {};
        $scope.recordCount = -1;
        $scope.reportExecuting = false;
        $scope.gridOptions = null;

        $scope.textFilters = function(filter) {
            return filter.DataTypeName == 'System.String' && filter.Options.length == 0;
        }

        $scope.listFilters = function (filter) {
            return filter.Options.length > 0;
        }

        $scope.resetFilters = function() {
            $scope.report.Filters.forEach(function(filter) {
                filter.Value = filter.DefaultValue;
            });
        }

        $scope.executeReport = function () {
            if ($scope.grid.dataSource.page() != 1)
                $scope.grid.dataSource.page(1);
            else
                $scope.grid.dataSource.read();
        }

        $scope.exportReport = function () {
            $scope.grid.saveAsExcel();
        }

        reportService.getReportMetaData($scope.reportId).then(function (data) {

            $scope.report = data;

            var columns = data.Columns.map(function (item) {
                var column = {
                    field: item.DataField,
                    title: item.Title,
                    width: item.Width,
                    template: item.Template,
                    sortable: item.IsSortable,
                    locked: item.IsLocked,
                    attributes: null,
                    headerAttributes: null
                };

                switch(item.Column.DataTypeName) {
                    case "System.DateTime":
                    case "System.Int32":
                    case "System.Decimal":
                        column.attributes = { 'class': 'text-right ' };
                        column.headerAttributes = { 'class': 'text-right ' };
                        break;
                }

                return column;

            });

            var options = {
                autoBind: false,
                sortable: data.IsSortable,
                pageable: data.IsPageable,
                scrollable: true,
                height: 500,
                columns: columns,
                excel: {
                    fileName: data.FullName + ".xlsx",
                    allPages: true
                },
                dataSource: {
                    schema: {
                        data: "Results",
                        total: "TotalRowCount"
                    },
                    serverPaging: true,
                    serverSorting: true,
                    pageSize: data.PageSize,
                    transport: {
                        read: function (e) {
                            var request = {
                                ViewID: $scope.report.ReportID,
                                Pager: e.data,
                                FormatResults : true,
                                Filters: $scope.report.Filters.map(function(f) {
                                    return {
                                        FilterID: f.FilterID,
                                        Value: f.Value
                                    }
                                })
                            };

                            $scope.reportExecuting = true;

                            reportService.executeReport(request).then(function (response) {
                                $scope.reportExecuting = false;
                                $scope.recordCount = response.TotalRowCount;
                                $timeout(function() { e.success(response); }, 200);
                            }, function() {
                                $scope.reportExecuting = false;
                            });
                        }
                    }
                }
            };

            if (data.IsSortable) {
                angular.extend(options, { sortable: { mode: 'single', allowUnsort: false } });
            }

            if (data.IsPageable) {
                angular.extend(options, { pageable: { refresh: false, pageSizes: false, buttonCount: 5 } });
            }

            $scope.gridOptions = options;
        });

}]);