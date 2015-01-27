
(function ($) {

    if (!window.kendo)
        return;

    var kendo = window.kendo,
        ui = kendo.ui,
        Grid = ui.Grid;

    var ExtGrid = Grid.extend({

        options: {
            name: "ExtGrid",
            resizeOffset: 5,
            footerSelector: "#footer",
            minHeight: 200
        },

        init: function (element, options) {
            var that = this,
                userDataBound = options.dataBound;

            // Re-wire databound event to do a resize automatically
            options.dataBound = function () {
                if (options.scrollable)
                    that._resize();

                if (userDataBound)
                    userDataBound.apply(that, arguments);
            };

            // Call the base class init.
            Grid.fn.init.call(that, element, options);

            // The Kendo library has some hard coded references to kendoGrid, so the following
            // needs to be added to make this work.
            $(element).data("kendoGrid", that);

            if (options.scrollabe) {
                $(window).resize(function () {
                    that._resize();
                });
            };
        },

        _resize: function() {
            var $grid = this.element,
                $footer = $(this.options.footerSelector),
                padding = this.options.resizeOffset,
                minHeight = this.options.minHeight,
                height = $(window).height() - $grid.offset().top - $footer.height() - padding;

            if (height < minHeight)
                height = minHeight;

            $grid.height(height);
            this._setContentHeight();
        }
    });

    ui.plugin(ExtGrid);

})(jQuery);