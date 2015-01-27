
(function ($) {

    if (!window.kendo)
        return;

    var kendo = window.kendo,
        ui = kendo.ui,
        ListView = ui.ListView;

    var ExtListView = ListView.extend({

        options: {
            name: "ExtListView"
        },

        init: function (element, options) {
            var that = this;

            kendo.ui.ListView.fn.init.call(that, element, options);

            $(element).data("kendoListView", that);
        },

        edit: function (item) {
            var that = this;

            kendo.ui.ListView.fn.edit.call(that, item);

            var $editRow = that.element.find(".k-edit-item");

            if ($editRow.length) {
                that.element.parent().scrollTop($editRow.position().top);
            }
        }
    });

    ui.plugin(ExtListView);

})(jQuery);