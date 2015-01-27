
(function ($, _) {

    var colorRanges = [
        { min: 0, max: 20, color: '#c41230' },
        { min: 21, max: 40, color: '#fd3702' },
        { min: 41, max: 60, color: '#fd7202' },
        { min: 61, max: 80, color: '#fdbf02' },
        { min: 81, max: 100, color: '#90fd02' }];

    $.widget("portal.progressBar", {

        options: {
            score: 0,
            title: '',
            titleUrl: '#',
            enabled: true,
            color: null,
            backgroundColor: '#195789',
            font: "300 48px 'Source Sans Pro'",
            fontColor: '#fff',
            tooltip: null,
            height: 190,
            width: 190,
            lineWidth: 20,
            animationSpeed: 1000,
            step: null
        },

        _create: function () {
            var $element = this.element;

            // Look for options set via data attributes
            for (var prop in this.options) {
                if ($element.data(prop) != null)
                    this.options[prop] = $element.data(prop);
            }

            // Set globals
            this.currentScore = -1;
            this.animationIntervalId = null;
            this.color = this._getColor(this.options.score);

            // Create html
            var html = "<a href='" + this.options.titleUrl + "'>" +
                         "<div class='progress'></div>" +
                         "<div class='text-wrap'>" +
                            "<span class='text'>" + this.options.title + "</span>" +
                         "</div>" +
                         "<div class='score-wrap'>" +
                            "<span class='score'>0 %</span>" +
                        "</div>" +
                       "</a>";

            $element.addClass("progressbar");
            $element.append(html);

            if (!this.options.enabled)
                $element.addClass("disabled");

            $element.find(".progress").css({ 'background-color': this.color });

            // Initialize animation
            this._animate();
        },

        _animate: function () {
            var that = this,
                interval = (this.options.animationSpeed / this.options.score),
                $progress = this.element.find(".progress"),
                $score = this.element.find(".score");

            // Just draw the end result if we're invisible
            if (!this.element.is(":visible")) {
                this.currentScore = this.options.score - 1;
            }

            this.animationIntervalId = setInterval(function () {
                that.currentScore++;

                $score.text(that.currentScore + ' %');
                $progress.css({ width: that.currentScore + '%' });

                if (that.options.step)
                    that.options.step.call(this, that.currentScore);

                if (that.currentScore >= that.options.score)
                    clearInterval(that.animationIntervalId);

            }, interval);
        },

        _getColor: function (score) {
            if (this.options.color && this.options.color.length)
                return this.options.color;

            var range = _.find(colorRanges, function (item) { return (score >= item.min && score <= item.max); });

            return range != null ? range.color : '#fd3702';
        },

        _destroy: function () {
            this.element.empty();
        }
    });

})(jQuery, _);