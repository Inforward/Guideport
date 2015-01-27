
(function (portal, $) {

    portal.dashboard = function () {

        function init() {

            $("[data-role='gauge']").gauge();

            $("[data-role='progressbar']").progressBar();

            $("[data-role='toggle']").on("click", toggle);

            $("[data-role='qlikviewFrame']").qlikviewFrame();
        }

        function toggle(e) {
            var $link = $(e.currentTarget),
                $parent = $link.parent(),
                $target = $($link.data("target")),
                expanded = $target.is(":visible");

            if (expanded) {
                $target.hide();
                $link.text("restore");
                $parent.addClass("closed");
                
            } else {
                $link.text("close");
                $parent.removeClass("closed");;
                $target.show();
            }

            expanded = !expanded;

            portal.dataManager.send({
                data: JSON.stringify({ expanded: expanded.toString() }),
                url: "pentameter/home/SaveBannerState"
            });
        }

        return {
            init: init
        };

    }();


}(this.portal = this.portal || {}, jQuery));