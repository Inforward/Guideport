
(function ($) {

    if (!window.kendo)
        return;

    var kendo = window.kendo,
        ui = kendo.ui,
        DatePicker = ui.DatePicker;

    var ExtDatePicker = DatePicker.extend({

        options: {
            name: "ExtDatePicker"
        },

        init: function (element, options) {
            var that = this;

            DatePicker.fn.init.call(that, element, options);

            // Do not allow manual input
            that.element.prop("readonly", "readonly");

            // If the user clicks the textbox, show the calendar
            that.element.on("click", $.proxy(that.open, that));

            $(element).data("kendoDatePicker", that);
        }
    });

    ui.plugin(ExtDatePicker);

})(jQuery);