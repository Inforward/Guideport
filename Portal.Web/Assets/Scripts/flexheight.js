
(function ($) {

    $.widget("portal.flexHeight", {

        options: {
            footer: '#footer',
            minHeight: 100,
            offset: 0
        },

        _create: function () {

            this.$footer = this.options.footer;
            this.$window = $(window);

            if (typeof this.$footer === "string") {
                this.$footer = $(this.$footer);
            }

            this.$window.resize($.proxy(this._resizeHeight, this));

            this._resizeHeight();
        },

        _resizeHeight: function() {
            var $element = this.element,
                height = this.$window.height() - $element.offset().top - this.$footer.outerHeight();

            if (this.options.offset > 0) {
                height = height - this.options.offset;
            }

            if (height < this.options.minHeight)
                height = this.options.minHeight;

            $element.height(height);

            var kendoGrid = $element.data("kendoGrid");

            if (kendoGrid)
                kendoGrid._setContentHeight();
        },

        _destroy: function () {
            this.$window.off("resize");
        }
    });

})(jQuery);