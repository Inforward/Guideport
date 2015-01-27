
(function (portal, $) {

    portal.timer = function () {
        var timerId = null;

        var config = {
            interval: 60000,
            timeout: null,
            activityDetected: null,
            resetOnActivity: true
        };
        
        function init(options) {
            $.extend(config, options);

            $(document).on("click", function() {
                if (config.activityDetected)
                    config.activityDetected.call(this);

                if (config.resetOnActivity)
                    reset();
            });

            reset();
        }

        function destroy() {
            clear();
            $(document).off("click", reset);
        }

        function clear() {
            if (timerId) window.clearTimeout(timerId);
        }

        function reset() {
            clear();
            timerId = window.setTimeout(expired, config.interval);
        }

        function expired() {
            if (config.timeout) 
                config.timeout.call(this);

            reset();
        }

        return {
            init: init,
            destroy: destroy,
            reset: reset,
            stop: clear
        };
    };

}(this.portal = this.portal || {}, jQuery));