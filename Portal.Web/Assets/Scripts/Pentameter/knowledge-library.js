
(function (portal, $, _) {

    portal.knowledgeLibrary = function () {
        var config = {},
            selectedTopic = "",
            subtopicDropDown,
            clearSubtopicButton;
        
        function init(options) {
            $.extend(config, options);
            initGrid();
        }

        function topicFilter(element) {
            element.kendoDropDownList({
                dataTextField: "Topic",
                dataValueField: "Topic",
                dataSource: config.topics,
                optionLabel: "--Select Topic--",
                change: function(e) {
                    selectedTopic = this.value();
                }
            });
        }

        function subTopicFilter(element) {
            element.kendoDropDownList({
                dataTextField: "Subtopic",
                dataValueField: "Subtopic",
                dataSource: config.subtopics,
                optionLabel: "--Select Subtopic--",
                open: function (e) {
                    if (selectedTopic != "") {
                        var subtopics = [];
                        $.each(config.subtopics, function(index, value) {
                            if (value.Topic == selectedTopic) {
                                subtopics.push(value);
                            }
                        });

                        subtopicDropDown.dataSource.data(subtopics);
                    }
                }
            });

            subtopicDropDown = element.data("kendoDropDownList");
        }

        function initGrid() {

            var gridConfig = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: config.url,
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
                            contains: "Contains",
                            startswith: "Starts With"
                        }
                    }
                },
                filterMenuInit: function (e) {
                    if (e.field == "Topic" || e.field == "Subtopic") {
                        e.container.find(".k-filter-help-text").hide();
                        e.container.find(".k-dropdown").first().hide();
                    }

                    if (e.field == "Topic") {
                        e.container.find(".k-button[type='reset']").click(function () {
                            selectedTopic = "";

                            if (clearSubtopicButton) {
                                clearSubtopicButton.trigger("click");
                            }
                        });
                    }

                    if (e.field == "Subtopic") {
                        clearSubtopicButton = e.container.find(".k-button[type='reset']");
                        clearSubtopicButton.click(function () {
                            if (selectedTopic == "") {
                                subtopicDropDown.dataSource.data(config.subtopics);
                            }
                        });
                    }
                },
                columns: [
                    { field: "Title", title: "Document Title", width: 230, filterable: true, template: "<a href='#=Permalink#' title='Click to download' target='_blank'>#=Title#</a>" },
                    { field: "Topic", title: "Measure", filterable: { ui: topicFilter } },
                    { field: "Subtopic", title: "Sub-topic", filterable: { ui: subTopicFilter } },
                    { field: "CreatedBy", title: "Created By", filterable: true } ]
            };

            // Finally, bind it
            $("#grid").kendoGrid(gridConfig);

            // Auto-adjust height
            $("#grid").flexHeight({ minHeight: 400 });
        }

        return {
            init: init
        };

    }();

}(this.portal = this.portal || {}, jQuery, _));
