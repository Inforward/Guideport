//
// Custom binder extensions for use with kendo templates/MVVM
//
(function ($) {
    
    if (window.kendo) {

        kendo.data.binders.slide = kendo.data.Binder.extend({
            refresh: function () {
                var value = this.bindings["slide"].get();

                if (value) {
                    $(this.element).slideDown();
                } else {
                    $(this.element).slideUp();
                }
            }
        });

        kendo.data.binders.fade = kendo.data.Binder.extend({
            refresh: function () {
                var value = this.bindings["fade"].get();

                if (value) {
                    $(this.element).fadeIn("fast");
                } else {
                    $(this.element).hide();
                }
            }
        });

        kendo.data.binders.widget.max = kendo.data.Binder.extend({
            init: function (widget, bindings, options) {
                kendo.data.Binder.fn.init.call(this, widget.element[0], bindings, options);
            },
            refresh: function () {
                var that = this,
                    value = that.bindings["max"].get(),
                    kendoDatePicker = $(that.element).data("kendoDatePicker");

                if (kendoDatePicker)
                    kendoDatePicker.max(value);
            }
        });

        kendo.data.binders.widget.min = kendo.data.Binder.extend({
            init: function (widget, bindings, options) {
                kendo.data.Binder.fn.init.call(this, widget.element[0], bindings, options);
            },
            refresh: function () {
                var that = this,
                    value = that.bindings["min"].get(),
                    kendoDatePicker = $(that.element).data("kendoDatePicker");

                if (kendoDatePicker)
                    kendoDatePicker.min(value);
            }
        });

        kendo.data.binders.saving = kendo.data.Binder.extend({
            refresh: function () {
                var value = this.bindings["saving"].get(),
                    $element = $(this.element),
                    $span = $element.find("span");

                if (!$span.length) {
                    $span = $("<span class='auto-save' />");
                    $element.append($span);
                }

                if (value === true) {
                    $element.addClass("saving").show();
                    $span.text("Saving changes...");
                }

                if(value === false) {
                    $element.removeClass("saving");
                    $span.text("All changes saved!");
                    window.setTimeout(function () { $element.fadeOut("slow"); }, 3000);
                }
            }
        });
    }

}(jQuery));