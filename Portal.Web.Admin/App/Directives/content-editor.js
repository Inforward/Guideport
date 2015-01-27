'use strict';

angular.module('admin.Directives').directive('contentEditor', function () {
    return {
        restrict: "AE",
        transclude: true,
        template: "<textarea kendo-editor k-options='editorOptions' ng-model='content'></textarea>",
        scope: {
            content: "="
        },
        controller: ["$scope", function ($scope) {

            var tools = [
                'bold', 'italic', 'underline', 'strikethrough',
                'justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull',
                'insertUnorderedList', 'insertOrderedList', 'indent', 'outdent',
                'createLink', 'unlink', 'insertImage', 'subscript',
                'superscript', 'createTable', 'addRowAbove', 'addRowBelow',
                'addColumnLeft', 'addColumnRight', 'deleteRow', 'deleteColumn',
                'viewHtml', 'formatting',
                {
                    name: "fontName",
                    items: [
                        { text: "Arial", value: "Arial" },
                        { text: "Source Sans Pro", value: "Source Sans Pro" },
                        { text: "Courier New", value: "Courier New" },
                        { text: "Verdana", value: "Verdana" },
                        { text: "Tahoma", value: "Tahoma" },
                        { text: "Times New Roman", value: "Times New Roman" },
                        { text: "Trebuchet MS", value: "Trebuchet MS" }
                    ]
                },
                {
                    name: "fontSize",
                    items: [
                        { text: "8px", value: "8px" },
                        { text: "10px", value: "10px" },
                        { text: "12px", value: "12px" },
                        { text: "14px", value: "14px" },
                        { text: "16px", value: "16px" },
                        { text: "18px", value: "18px" },
                        { text: "20px", value: "20px" },
                        { text: "22px", value: "22px" },
                        { text: "24px", value: "24px" },
                        { text: "26px", value: "26px" },
                        { text: "28px", value: "28px" },
                        { text: "30px", value: "30px" },
                        { text: "32px", value: "32px" },
                        { text: "36px", value: "36px" },
                        { text: "48px", value: "48px" },
                        { text: "72px", value: "72px" }
                    ]
                },
                'foreColor', 'backColor',
                {
                    name: "horizontalRule",
                    tooltip: "Insert a horizontal rule",
                    exec: function () {
                        $(this).data("kendoEditor").exec("inserthtml", { value: "<hr />" });
                    }
                },
                {
                    name: "suggestedReading",
                    tooltip: "Insert a suggested reading",
                    exec: function () {
                        var html =
                            "<span>&nbsp;</span>" +
                            "<div class='row suggested-reading'>" +
                              "<div class='col-sm-3 image'>" +
                                "<img alt='' src='http://ecx.images-amazon.com/images/I/51QibTfTHzL._AA160_.jpg' />" +
                              "</div>" +
                              "<div class='col-sm-9 info'>" +
                                "<h4>Book Title Goes Here</h4>" +
                                "<p class='author'>by Author Name</p>" +
                                "<p class='published'>Published: Jan 1, 2014</p>" +
                                "<p class='description'>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam a odio ac ligula accumsan varius. Aenean molestie ligula ut blandit molestie. Mauris pharetra dictum diam vel pharetra. Etiam magna leo, aliquam nec scelerisque at, vulputate quis sapien.</p>" +
                                "<a href='http://www.amazon.com/dp/0814414168/' target='_blank'>View on Amazon</a>" +
                              "</div>" +
                            "</div>" +
                            "<hr />";

                        $(this).data("kendoEditor").exec("inserthtml", { value: html });
                    }
                }
            ];


            $scope.editorOptions = {
                tools: tools,
                encoded: false,
                serialization: {
                    scripts: true
                },
                stylesheets: [
                    '//fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic,700italic',
                    '/Assets/Css/bootstrap.css',
                    '/Assets/Css/editor.css'
                ]
            };
        }]
    };
});