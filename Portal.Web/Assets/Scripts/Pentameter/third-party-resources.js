
(function (portal, $, _) {

    portal.thirdPartyResources = function () {
        var $grid,
            $modal;

        var config = {
            baseUrl: '',
            tooltipTemplate: '',
            services: []
        };

        function init(options) {
            $.extend(config, options);

            $grid = $("#grid");
            $modal = $("#service-modal");

            $("[data-role='rolodex']").rolodex({
                click: rolodexClick
            });

            initGrid();
            initTooltips();
        }

        function rolodexClick(text) {
            var kendoGrid = $grid.data("kendoGrid"),
                dataSource = kendoGrid.dataSource,
                filter = dataSource.filter() || {},
                currentFilters = filter.filters || [];

            // Remove existing filter for ResourceName
            var filters = _.reject(currentFilters, function(f) { return f.field == 'Name'; });

            // Add ResourceName filter
            if (text != 'all') {
                filters.push({
                    field: "Name",
                    operator: "startswith",
                    value: text
                });
            }

            // Apply filters
            dataSource.filter(filters);
        }

        function servicesFilter(element) {
            element.kendoDropDownList({
                dataTextField: "ServiceName",
                dataValueField: "ServiceName",
                dataSource: config.services,
                optionLabel: "--Select Service--"
            });
        }

        function initTooltips() {
            var template = kendo.template($(config.tooltipTemplate).html());

            $grid.on("click", "a.service-tooltip", function(e) {
                var $row = $(e.currentTarget).closest("tr"),
                    dataItem = $grid.data("kendoGrid").dataItem($row),
                    html = template(dataItem);

                $modal.find(".modal-title").text(dataItem.Name);
                $modal.find(".modal-body").html(html);
                $modal.modal('show');
            });
        }

        function initGrid() {

            var gridConfig = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: config.baseUrl + "/JsonGetThirdPartyResources",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json"
                        }
                    }
                }),
                pageable: false,
                sortable: { mode: 'single', allowUnsort: false },
                height: 430,
                filterable: {
                    extra: false,
                    operators: {
                        string: {
                            contains: "Contains"
                        }
                    }
                },
                filterMenuInit: function (e) {
                    if (e.field == "Services") {
                        e.container.find(".k-filter-help-text").hide();
                        e.container.find(".k-dropdown").first().hide();
                    }
                },
                columns: [
                    { field: "Name", title: "Company Name", width: 200, filterable: false, template: "<a class='service-tooltip pointer' title='Click to view details'>#=Name#</a>" },
                    { title: "Services", field: "Services", filterable: { ui: servicesFilter } },
                    { field: "Description", hidden: true },
                    { field: "AddressLine1", hidden: true },
                    { field: "AddressLine2", hidden: true },
                    { field: "City", hidden: true },
                    { field: "State", hidden: true },
                    { field: "PostalCode", hidden: true },
                    { field: "Country", hidden: true },
                    { field: "PhoneNo", hidden: true },
                    { field: "FaxNo", hidden: true },
                    { field: "Email", hidden: true },
                    { field: "WebsiteUrl", hidden: true }
                ]
            };

            // Bind it
            $grid.kendoGrid(gridConfig);

            // Auto-adjust height
            $grid.flexHeight({
                minHeight: 430
            });
        }

        return {
            init: init
        };

    }();

}(this.portal = this.portal || {}, jQuery, _));