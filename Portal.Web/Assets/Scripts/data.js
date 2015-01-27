
(function (portal, $, undefined) {

    portal.dataManager = {
        dataDefaults: {
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false
        },

        send: function (options) {
            var that = portal.dataManager,
                normalizedUrl = that.getEndpointUrl(options.url),
                callerOptions = $.extend({}, that.dataDefaults, options, { url: normalizedUrl }),
                userSuccess = callerOptions.success;

            var success = function(data, textStatus, jqXHR) {
                if (data && data.hasOwnProperty("Success") && data.Success === false) {
                    alert(data.Message);
                    return;
                }

                var userData = data;

                if (data && data.hasOwnProperty("Data"))
                    userData = data.Data;

                if (userSuccess)
                    userSuccess(userData, textStatus, jqXHR);
            }

            $.extend(callerOptions, { success: success });

            return $.ajax(callerOptions);
        },

        getEndpointUrl: function (url) {
            if (url) {
                var startsWith = url.slice(0, portal.rootUrl.length) == portal.rootUrl;

                if (!startsWith)
                    url = portal.rootUrl + url;
            }

            return url;
        }
    };

}(this.portal = this.portal || {}, jQuery));
