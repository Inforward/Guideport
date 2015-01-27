
(function (portal, $) {

    portal.sessionMonitor = function () {

        var config = {
            sessionText: 'session'
        };

        function dialog(options) {
            var $dialog = $("<div style='padding: 20px;text-align: center;'><p style='margin-bottom: 30px'>" + options.text + "</p><a class='btn btn-primary' style='width: 75px;'>Ok</a></div>");

            $dialog.on("click", ".btn", function() {
                if (options.ok)
                    options.ok.call(this);
            });

            $dialog.kendoWindow({
                title: options.title,
                resizable: false,
                modal: true,
                actions: []
            });

            return {
                show: function() {
                    $dialog.data("kendoWindow").center().open();
                },

                hide: function() {
                    $dialog.data("kendoWindow").close();
                }
            };
        }


        function init(options) {
            $.extend(config, options);

            var warningTimer = new portal.timer(),
                expirationTimer = new portal.timer();

            // Create dialogs
            var warningDialog = new dialog({
                text: "Your " + config.sessionText + " is about to expire.  Click Ok to continue.",
                title: "Inactivity Warning",
                ok: function() {
                    warningDialog.hide();
                    warningTimer.reset();
                    expirationTimer.reset();
                }
            });

            var expiredDialog = new dialog({
                text: "Your " + config.sessionText + " has expired.  Click Ok to restart your session.",
                title: "Session Expired",
                ok: function () {
                    window.location.reload();
                }
            });
            
            // Initialize timers
            warningTimer.init({
                interval: portal.config.sessionMonitor.warningTimeout,
                timeout: function () {
                    // Stop the timer
                    warningTimer.stop();
                    
                    // Show dialog
                    warningDialog.show();
                }
            });

            expirationTimer.init({
                interval: portal.config.sessionMonitor.expirationTimeout,
                timeout: function () {
                    // Hide warning dialog
                    warningDialog.hide();

                    // Destroy timers
                    warningTimer.destroy();
                    expirationTimer.destroy();

                    // Show expiration dialog
                    expiredDialog.show();
                }
            });
        }

        return {
            init: init
        };

    }();

}(this.portal = this.portal || {}, jQuery));