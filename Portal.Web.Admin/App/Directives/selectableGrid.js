'use strict';

angular.module('admin.Directives').directive('selectableGrid', function () {
    return {
        restrict: 'A',
        scope: {
            kOptions: '=',
            items: '='
        },
        link: function (scope, element, attrs) {

            var unregister = scope.$watch("kOptions", function(newValue, oldValue) {
                if (newValue == null)
                    return;

                var options = newValue,
                    readOnly = options.readOnly || false;

                if (!readOnly) {

                    options.columns.unshift({
                        template: "<input type='checkbox' />",
                        headerTemplate: "<input type='checkbox' title='Select all' class='grid-select-all' />",
                        width: 30
                    });

                    var userDataBound = options.dataBound;

                    options.dataBound = function(e) {
                        element.find("input:checkbox").prop("checked", false);
                        scope.items = [];

                        if (userDataBound)
                            userDataBound.call(this, e);
                    }

                    unregister();
                }
            });

            function processItem(id, add) {
                if (add) {
                    scope.items.push(id);
                } else {
                    var index = scope.items.indexOf(id);
                    if (index != -1)
                        scope.items.splice(index, 1);
                }
                scope.$apply();
            }

            element.on("click", ".grid-select-all", function (e) {
                var checkbox = $(e.target),
                    checked = checkbox.is(":checked"),
                    grid = checkbox.closest('.k-grid').data("kendoGrid"),
                    checkboxes = checkbox.closest('.k-grid').find(".k-grid-content input:checkbox");

                $.each(checkboxes, function () {
                    var $this = $(this),
                        $row = $this.closest("tr"),
                        dataItem = grid.dataItem($row);

                    $this.prop("checked", checked);

                    if (checked) {
                        $row.addClass("k-state-selected");
                    } else {
                        $row.removeClass("k-state-selected");
                    }

                    processItem(dataItem.id, checked);
                });
            });

            element.on("click", "input:checkbox:not(.grid-select-all)", function (e) {
                var checkbox = $(e.target),
                    checked = checkbox.is(":checked"),
                    row = checkbox.closest("tr"),
                    grid = element.closest('[kendo-grid]').data("kendoGrid"),
                    dataItem = grid.dataItem(row);

                if (checked) {
                    row.addClass("k-state-selected");
                } else {
                    row.removeClass("k-state-selected");
                }

                processItem(dataItem.id, checked);
            });

        }
    }
});