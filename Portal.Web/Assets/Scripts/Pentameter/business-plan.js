
(function (portal, $) {

    portal.businessPlan = function () {

        var config = {
            baseUrl: "/",
            businessPlanId: null,
            employees: []
        };

        function init(options) {
            var $main = $("#main");

            $.extend(config, options);

            kendo.init($main);

            $("[data-role='slider']").each(function () {
                var kendoSlider = $(this).data("kendoSlider");
                kendoSlider.bind("slide", updateSliderLabel);
                kendoSlider.bind("change", sliderChanged);
            });

            $("#orgchart").kendoOrgChart({
                dataSource: config.employees,
                enabled: false,
                keyField: "EmployeeID",
                parentField: "EmployeeParentID",
                nameField: "FullName",
                titleField: "Roles"
            });

            // Fix 'undefined' titles on drag handles...
            $main.find(".k-draghandle").attr("title", "Update");

            // Handle Tactic Completion
            $("span.checkmark").on("click", function(e) {
                var $element = $(e.currentTarget);

                $element.toggleClass("checked");

                var isComplete = $element.hasClass("checked"),
                    tooltip = isComplete ? "Click to mark incomplete." : "Click to mark complete.",
                    tacticId = $element.data("tacticId");

                $element.attr("title", tooltip);

                portal.dataManager.send({
                    async: false,
                    url: config.baseUrl + "/JsonUpdateTacticStatus",
                    data: kendo.stringify({ tacticId: tacticId, isComplete: isComplete })
                });

            });
        }

        function sliderChanged(e) {
            var objectiveId = e.sender.element.data("objectiveId");

            updateSliderLabel(e);

            portal.dataManager.send({
                async: false,
                url: config.baseUrl + "/JsonUpdateObjectiveStatus",
                data: kendo.stringify({
                    objectiveId: objectiveId,
                    percentComplete: e.value
                })
            });
        }

        function updateSliderLabel(e) {
            var value = kendo.format("{0} %", e.value),
                labelContainer = e.sender.wrapper.parent().find(".label-wrap"),
                label = labelContainer.find("label");

            label.text(value);

            if (e.value == 100) {
                labelContainer.addClass("complete");
            } else {
                labelContainer.removeClass("complete");
            }
        }

        return {
            init: init
        };

    }();


}(this.portal = this.portal || {}, jQuery));