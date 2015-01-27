
(function ($, _) {

    var colorRanges = [
        { min: 0,  max: 20, color: '#c41230' },
        { min: 21, max: 40, color: '#fd3702' },
        { min: 41, max: 60, color: '#fd7202' },
        { min: 61, max: 80, color: '#fdbf02' },
        { min: 81, max: 100, color: '#90fd02'}];

    $.widget("portal.gauge", {

        options: {
            score: 0,
            title: null,
            titleUrl: null,
            enabled: true,
            color: '#fd3702',
            backgroundColor: '#195789',
            font: "300 48px 'Source Sans Pro'",
            fontColor: '#fff',
            tooltip: null,
            height: 190,
            width: 190,
            lineWidth: 20,
            animate: true,
            animationSpeed: 1000,
            context: null
        },

        _create: function () {
            var that = this,
                options = that.options,
                element = this.element;

            // Look for options set via data attributes
            for (var prop in this.options) {
                if (element.data(prop) != null)
                    this.options[prop] = element.data(prop);
            }

            // Set globals
            this.currentScore = -1;
            this.animationIntervalId = null;
            this.gaugeColor = this._getColor(options.score);

            // Create html
            var html = "<canvas width='" + options.width + "' height='" + options.height + "' />";

            if (options.titleUrl && options.titleUrl.length)
                html = "<a href='" + options.titleUrl + "'>" + html + "</a>";

            if (options.title && options.title.length) {
                html += "<div class='title-wrap'>" +
                           "<span class='fa fa-info-circle info' data-toggle='tooltip'></span>" +
                           "<a href='" + options.titleUrl + "'>" + options.title + "</a>" +
                        "</div>";
            }

            element.addClass("gauge");
            element.append(html);

            if (!options.enabled)
                element.addClass("disabled");

            // Initialize tooltips
            var $tooltip = element.find(".info");

            if (options.tooltip && options.tooltip.length) {
                $tooltip.tooltip({
                    trigger: 'hover click',
                    title: options.tooltip,
                    container: 'body',
                    html: true
                });
            } else {
                $tooltip.hide();
            }

            // Initialize animation
            this._animateGauge();
        },

        _animateGauge: function () {
            var that = this,
                interval = (that.options.animationSpeed / that.options.score);

            // Just draw the end result if we're invisible (or animation is disabled)
            if (!this.element.is(":visible") || this.options.animate === false) {
                this.currentScore = this.options.score;
                this._drawGauge();
                return;
            }
            
            that.animationIntervalId = setInterval(function () {
                that.currentScore++;
                that._drawGauge();

                if (that.currentScore >= that.options.score)
                    clearInterval(that.animationIntervalId);

            }, interval);
        },

        _drawGauge: function () {
            var canvas = this.element.find("canvas")[0],
                ctx = canvas.getContext("2d"),
                width = canvas.width,
                height = canvas.height,
                radius = (width / 2) - this.options.lineWidth,
                text = this.currentScore + "%",
                context = this.options.context;

            // Show something even if score is 0
            //if (this.options.enabled && this.options.score == 0) {
            //    this.currentScore = 1;
            //    text = "0%";
            //}

            var degrees = Math.floor((this.currentScore / 100) * 360),
                radians = degrees * Math.PI / 180;

            // Clear the canvas
            ctx.clearRect(0, 0, width, height);

            // Draw 'background' arc
            ctx.beginPath();
            ctx.arc(width / 2, height / 2, radius, 0, Math.PI * 2, false);
            ctx.fillStyle = this.options.backgroundColor;
            ctx.fill();
            ctx.lineWidth = this.options.lineWidth;
            ctx.strokeStyle = '#99b5cb';
            ctx.stroke();

            // Draw progress arc over background
            ctx.beginPath();
            ctx.strokeStyle = this.gaugeColor;
            ctx.lineWidth = this.options.lineWidth;
            ctx.arc(width / 2, height / 2, radius, .5 * Math.PI, radians + 90 * Math.PI / 180, false);
            ctx.stroke();

            // Add the text
            ctx.fillStyle = this.options.fontColor;
            ctx.font = this.options.font;

            var y = (height / 2) + 15;

            if (context && context.length)
                y -= 10;

            ctx.fillText(text, (width / 2) - (ctx.measureText(text).width / 2), y);

            if (context && context.length) {
                ctx.font = "300 16px 'Source Sans Pro'",
                ctx.fillText(context, (width / 2) - (ctx.measureText(context).width / 2), y + 20);
            }
        },

        _getColor: function (score) {
            var range = _.find(colorRanges, function (item) { return (score >= item.min && score <= item.max); });

            return range != null ? range.color : '#fd3702';
        },

        _destroy: function () {
            this.element.empty();
        }
    });

})(jQuery, _);