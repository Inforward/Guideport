
(function (portal, $) {


    //
    // Global initialization/bootstrapping
    //
    $(function () {

        // Handle ajax redirects (308 is a custom code set in Global.asax)
        $(document).ajaxError(function (e, request) {
            if (request.status == "308") {
                window.location = request.getResponseHeader('location');
            }
        });

        // Init mobile (slide) menu
        $("#slide-nav").mmenu({
            slidingSubmenus: false
        });

        // Initialize Search Form
        initializeSearchForm();

        // Init our keep-alive ping-er
        if (portal.keepAlive && portal.config && portal.config.keepAlive)
            portal.keepAlive.init();

    });
    
    function initializeSearchForm() {
        var $form = $('#quick-search'),
            $searchText = $form.find("input[name='q']"),
            $selectedSite = $form.find("input[name='s']");

        if (!$form.length)
            return;

        $form.find(".dropdown-menu li").on("click", function(e) {
            var site = $(e.currentTarget).text();

            $selectedSite.val(site);
            $searchText.attr("placeholder", site);
        });
    }

}(this.portal = this.portal || {}, jQuery));