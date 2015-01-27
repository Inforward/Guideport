
(function (portal, $) {

    $.widget("portal.termsDialog", {
        options: {
            validateUrl: null,
            acceptUrl: null,
            onAccepted: null,
            termsContent: null
        },

        _create: function () {
            var that = this;

            for (var prop in that.options) {
                if (that.element.data(prop) != null)
                    that.options[prop] = that.element.data(prop);
            }
        },

        _init: function () {

            this._validateTerms();

        },

        _validateTerms: function () {
            var that = this;

            portal.dataManager.send({
                url: that.options.validateUrl,
                success: function (data) {
                   if (data.Accepted) {
                       that._accepted();
                   } else {
                       that._showDialog();
                   }
                }
            });
        },

        _accept: function () {
            var that = this;

            portal.dataManager.send({
                url: that.options.acceptUrl,
                success: function () {
                    that.$dialog.modal('hide');
                    that._accepted();
                }
            });
        },

        _accepted: function () {
            var acceptedFn = this.options.onAccepted;

            if (acceptedFn && typeof (acceptedFn) == "function") {
                acceptedFn.call(this);
            }
        },

        _showDialog: function () {
            var termsSelector = typeof (this.options.termsContent) == "string" ? $(this.options.termsContent) : this.options.termsContent,
                terms = termsSelector.html();

            var html =
                "<div class='modal fade' tabindex='-1' role='dialog' aria-hidden='true' data-backdrop='static' data-keyboard='false'>" +
                    "<div class='modal-dialog'>" +
                        "<div class='modal-content'>" +
                            "<div class='modal-header'><h4 class='modal-title'>Cetera Financial Group</h4></div>" +
                            "<div class='modal-body'>" +
                                "<div class='business-valuation-terms-modal' style='text-align: left;'>" +
                                    "<div style='max-height: 450px; height: 100%; overflow-y: auto;'>" + terms + "</div>" +
                                    "<div style='text-align: center;'>" +
                                        "<p><a class='btn btn-primary btn-lg btn-icon' href='#'>I Agree</a></p>" +
                                        "<p class='back'><a href='javascript: history.back();'>No thanks, take me back.</a></p>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                "</div>";

            this.$dialog = $(html);
            this.$dialog.find(".btn-primary").on("click", $.proxy(this._accept, this));
            this.$dialog.modal("show");
        }
    });

})(this.portal = this.portal || {}, jQuery);

