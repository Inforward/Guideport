
(function (portal, $, _) {

    portal.search = function () {
        var viewModelObservable;

        var config = {
            baseUrl: '/Search',
            pageSize: 10,
            textMaxChars: 250,
            documentTypeIconClass: null
        };

        var viewModel = {
            data: [],
            isSearching: false,
            hasSearched: false,
            searchResultText: function() {
                var resultCount = this.hasData();

                if (resultCount == 0) {
                    return "";
                }
                else if (resultCount == 1) {
                    return "We found 1 result";
                }
                else {
                    return kendo.format("We found {0} results", resultCount);
                }
            },

            noResults: function () {
                return this.get("hasSearched") && !this.hasData();
            },

            criteria: {
                portalName: "",
                searchText: "",
                textMaxChars: 100
            },

            search: function() {
                var that = this;

                this.set("criteria.searchText", $("input[name='q']").val());
                this.set("criteria.siteName", $("input[name='s']").val());
                this.set("criteria.textMaxChars", config.textMaxChars);
                this.set("isSearching", true);

                portal.dataManager.send({
                    url:  config.baseUrl + "/JsonSearch",
                    data: kendo.stringify(this.criteria),
                    success: function (data) {
                        that.set("data", new kendo.data.DataSource({ data: data, pageSize: config.pageSize }));
                   }
                }).always(function () {
                    that.set("isSearching", false);
                }).done(function () {
                    that.set("hasSearched", true);
                });
            },

            hasData: function () {
                return this.get("data").options != null ? this.get("data").options.data.length : 0;
            }
        };

        function init(options) {

            $.extend(config, options);

            viewModelObservable = kendo.observable(viewModel);
            viewModelObservable.search();

            kendo.bind($("#results"), viewModelObservable);

            $("[data-role='listview']")
                .data("kendoListView")
                .bind("dataBound", highlightSearchTerm);
        }
        
        function highlightSearchTerm(e) {
            var searchTerms = $("input[name='q']").val().toLowerCase();
            var elements = $("[class^='item-']");
            var searchTermExp = new RegExp("([\\\"\"\\''])(.+?)([\\\"\"\\''])|[^ ,]+", "gi");
            var match, pattern = "";

            while (match = searchTermExp.exec(searchTerms)) {
                var searchTerm = match[2] != null ? match[2] : match[0];

                if (pattern != "")
                    pattern += "|";

                pattern += searchTerm.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
            }

            var highlightExp = new RegExp("(" + pattern + ")", "gi");

            elements.each(function () {
                var text = $(this).text();
                var html = text.replace(highlightExp, "<span class='highlight'>$1</span>");

                $(this).html(html);
            });
        }

        function pageChanged() {
            window.scrollTo(0, 0);
        }

        return {
            init: init,
            pageChanged: pageChanged
        };

    }();

}(this.portal = this.portal || {}, jQuery, _));
