
(function ($) {

    $.widget("portal.qlikviewFrame", {
        options: {
            id: null,
            documentName: null,
            load: null,
            error: null,
            timeout: 0,
            height: 0,
            width: 0,
            errorMessage: null,
            errorClass: "ajaxError",
            loadingText: "Loading...",
            loadingClass: "ajax-loading",
            loadingImage: "/Assets/Images/ajax-loader.gif"
        },

        timeoutError: null,

        _create: function () {

            // Look for options set via data attributes
            for (var prop in this.options) {
                if (this.element.data(prop) != null)
                    this.options[prop] = this.element.data(prop);
            }

            // Create "Loading" content
            this.$loader = $("<div class='" + this.options.loadingClass + "' style='display: none'>" +
                              "<img src='" + this.options.loadingImage + "' alt='loading' /><span>" + this.options.loadingText + "</span>" +
                             "</div>");

            // Add it
            this.element.append(this.$loader);

            // Show it
            this.$loader.fadeIn();

            this._fetchUrl();

            if (this.options.timeout > 0) {
                this.timeoutError = setTimeout($.proxy(this._error, this), this.options.timeout);
            }
        },

        _validateUrl: function (url) {
            if (/^([a-z]([a-z]|\d|\+|-|\.)*):(\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?((\[(|(v[\da-f]{1,}\.(([a-z]|\d|-|\.|_|~)|[!\$&'\(\)\*\+,;=]|:)+))\])|((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=])*)(:\d*)?)(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*|(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)|((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)|((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)){0})(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(url))
                return true;
            else
                return false;
        },

        _fetchUrl: function () {
            var that = this;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                data: JSON.stringify({ documentName: this.options.documentName }),
                url: portal.rootUrl + "pentameter/home/GetQlikViewUrl"

            }).done(function (response) {

                if (response.Success === false || !that._validateUrl(response.Data)) {
                    that._error();
                    return;
                }

                that._buildFrame(response.Data);

            }).fail(function() {
                that._error();

            });
        },

        _error: function () {
            if (this.timeoutError)
                clearTimeout(this.timeoutError);

            this.element.empty();
            this.element.append("<div class='" + this.options.errorClass + "'>" + this.options.errorMessage + "</div>");

            if (this.options.error)
                this.options.error.call(this);
        },

        _buildFrame: function (url) {
            var that = this;

            // Create html
            var $iframe = $("<iframe class='qlikview-frame' " +
                             "style='visibility: hidden; width: " + this.options.width + "; height: " + this.options.height + ";'" +
                             "src='" + url + "'>" +
                            "</iframe>");

            this.element.append($iframe);

            $iframe.load(function () {
                if (that.timeoutError)
                    clearTimeout(that.timeoutError);
                
                if (that.options.load)
                    that.options.load.call(this);

                that.$loader.fadeOut(function () {
                    $iframe.css({ "visibility": "visible" });
                });
                
            });
        },

        _destroy: function () {
            this.element.empty();
        }
    });

})(jQuery);