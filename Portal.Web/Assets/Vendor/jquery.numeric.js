
(function ($) {

    $.browser = {};
    $.browser.mozilla = /mozilla/.test(navigator.userAgent.toLowerCase()) && !/webkit/.test(navigator.userAgent.toLowerCase());
    $.browser.webkit = /webkit/.test(navigator.userAgent.toLowerCase());
    $.browser.opera = /opera/.test(navigator.userAgent.toLowerCase());
    $.browser.msie = /msie/.test(navigator.userAgent.toLowerCase());

    $.fn.numericOnly = function(options) {
        var opts = $.extend({ allowDecimal: true }, options);

        return this.each(function() {
            if (!$.browser.mozilla)
                $(this).keypress(function(e) {
                    var key = e.keyCode,
                        text = String.fromCharCode(key);

                    !text.match(/[0-9.]/) && e.preventDefault();

                });
            else
                $(this).keydown(function(e) {
                    var key = e.keyCode,
                        c = key >= 48 && key <= 57 || key >= 96 && key <= 105 || (key == 8 || key == 46 || key == 13 || key == 37 || key == 39 || key == 9);

                    !c && e.preventDefault();
                });
        });
    };
})(jQuery);