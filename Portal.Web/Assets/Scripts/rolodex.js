
(function ($) {

    $.widget("portal.rolodex", {

        options: {
            dataSource: ['all', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'],
            click: null
        },

        _create: function () {

            this.element.addClass("rolodex");

            this.element.on("click", "a", $.proxy(this._click, this));

            this._refresh();
        },

        _click: function(e) {
            var $link = $(e.currentTarget),
                text = $link.text();

            this.element.find("a.active").removeClass("active");

            $link.addClass("active");

            if (this.options.click)
                this.options.click.call(this, text);
        },

        _refresh: function() {
            this.element.empty();

            var html = "";

            $.each(this.options.dataSource, function(idx, item) {
                html += "<a>" + item + "</a>";
            });

            this.element.html(html);
            this.element.find("a").first().addClass("active");
        },

        _destroy: function () {
            $element.empty().removeClass("rolodex");
        }
    });

})(jQuery);