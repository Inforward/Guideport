
angular.module('admin.Directives')
    .directive('uiZeroclip', ['$document', '$window', function ($document, $window) {

        var config = {
            swfPath: "/Assets/Flash/ZeroClipboard.swf"
        };

        var ZeroClipboard = $window.ZeroClipboard;

        return {
            scope: {
                onCopied: '&zeroclipCopied',
                client: '=?uiZeroclip',
                value: '=zeroclipModel',
                text: '@zeroclipText'
            },
            link: function (scope, element, attrs) {

                var tooltip = 'Copy url to clipboard';

                element.tooltip({
                    trigger: 'manual',
                    title: tooltip,
                    container: 'body'
                });

                ZeroClipboard.config(config);

                var clip = new ZeroClipboard(element[0]);

                clip.on("ready", function () {
                    this.on("aftercopy", function () {
                        element.attr('data-original-title', 'copied!').tooltip('fixTitle').tooltip('show');
                    });
                });

                clip.on("mouseover", function () {
                    element.tooltip('show');
                });

                clip.on("mouseout", function () {
                    element.tooltip('hide').attr('data-original-title', tooltip).tooltip('fixTitle');
                });

                clip.on("error", function (e) {
                    element.hide();
                    console.log(e.name + ": " + e.message);
                });

                scope.$watch('value', function (v) {
                    if (v === undefined) {
                        return;
                    }
                    element.attr('data-clipboard-text', v);
                });

                scope.$watch('text', function (v) {
                    element.attr('data-clipboard-text', v);
                });

                //scope.$on('$destroy', function () {
                //    client.off('complete', _completeHnd);
                //});
            }
        };
    }
]);