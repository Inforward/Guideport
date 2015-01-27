
(function (portal, $) {

    portal.advisorView = function () {

        var viewModel = {
            criteria: {
                firstName: "",
                lastName: "",
                groupId: "",
                affiliateId: ""
            },
            defaultGroupId: "",
            grid: {},
            data: [],
            isSearching: false,
            hasSearched: false,
            noResults: function () {
                return this.get("hasSearched") && !this.hasData();
            },
            hasData: function () {
                return this.get("data").length;
            },
            clearCriteria: function(e) {
                e.preventDefault();

                this.set("criteria.firstName", "");
                this.set("criteria.lastName", "");
                this.set("criteria.groupId", this.get("defaultGroupId"));
                this.set("criteria.affiliateId", 0);
            },
            search: function (e) {
                var that = this,
                    grid = that.get("grid");

                e.preventDefault();

                if (grid.dataSource.page() != 1)
                    grid.dataSource.page(1);
                else {
                    that.set("isSearching", true);

                    grid.dataSource.read();
                }
            }
        };

        function init(options) {

            var obj = kendo.observable(viewModel);

            obj.set("criteria.groupId", options.defaultGroupId);
            obj.set("defaultGroupId", options.defaultGroupId);

            kendo.bind($("#advisor-view"), obj);

            obj.grid = $("#advisorViewGrid").kendoGrid({
                autoBind: false,
                columns: [
                    { field: 'DisplayName', title: 'Name', template: '<a href=\'/AdvisorView/StartSession/#=UserId#\'>#=DisplayName#</a>', width: 200 },
                    { field: 'BusinessConsultantDisplayName', title: 'Business Consultant', width: 180 },
                    { field: 'City', title: 'Location', width: 180, template: '#= portal.advisorView.formatCityState(City, State) #' },
                    { field: 'PrimaryPhone', title: 'Phone No', width: 140, template: '#= (PrimaryPhone) ? portal.advisorView.formatPhoneNumber(PrimaryPhone) : "" #' },
                    { field: 'AffiliateName', title: 'Affiliate', width: 180, hidden: options.isRestricted }
                ],
                height: 400,
                scrollable: true,
                sortable: { mode: 'single', allowUnsort: false },
                pageable: { refresh: false, pageSizes: false, buttonCount: 5 },
                dataSource: {
                    serverPaging: true,
                    serverSorting: true,
                    pageSize: 250,
                    schema: {
                        data: "Results",
                        total: "Total"
                    },
                    transport: {
                        read: {
                            url: "advisorview/search",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            cache: false
                        },
                        parameterMap: function(data, type) {
                            return kendo.stringify($.extend({}, obj.criteria, data));
                        }
                    },
                    change: function(data) {
                        obj.set("data", data.items);

                        if (obj.get("isSearching")) {
                            obj.set("isSearching", false);
                            obj.set("hasSearched", true);
                        }
                    }
                }
            }).data("kendoGrid");
        }

        function formatPhoneNumber(phoneNumber) {
            var piece1 = phoneNumber.substring(0, 3);
            var piece2 = phoneNumber.substring(3, 6);
            var piece3 = phoneNumber.substring(6);

            return kendo.format("({0}) {1}-{2}", piece1, piece2, piece3);
        }

        function formatCityState(city, state) {
            var cityState = "";

            if (city)
                cityState = city.replace(/(?:^|\s)\S/g, function (a) { return a.toUpperCase(); });

            if (state) {
                if (cityState)
                    cityState += ", ";

                cityState += state.toUpperCase();
            }

            return cityState;
        }

        return {
            init: init,
            formatPhoneNumber: formatPhoneNumber,
            formatCityState: formatCityState
        };

    }();

}(this.portal = this.portal || {}, jQuery));