
(function (portal, $) {

    portal.keepAlive = function () {
        var activityDetected = false,
            config = {};

        function ping() {
            $.ajax({
                url: config.url,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false
            });
        }

        function init() {
            var timer = new portal.timer();

            if(portal.config && portal.config.keepAlive)
                config = portal.config.keepAlive;

            timer.init({
                interval: config.interval,
                resetOnActivity: false,

                activityDetected: function() {
                    activityDetected = true;
                },

                timeout: function() {
                    if (!activityDetected)
                        return;

                    activityDetected = false;

                    ping();
                }
            });
        }

        return {
            init: init,
            ping: ping
        };

    }();

}(this.portal = this.portal || {}, jQuery));