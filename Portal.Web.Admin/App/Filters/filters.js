'use strict';

angular.module('admin.Filters').filter('phone', function () {
    return function (tel) {
        if (!tel) { return ''; }

        var value = tel.toString().trim().replace(/^\+/, '');

        if (value.match(/[^0-9]/)) {
            return tel;
        }

        var country, city, number;

        switch (value.length) {
            case 10: // +1PPP####### -> C (PPP) ###-####
                country = 1;
                city = value.slice(0, 3);
                number = value.slice(3);
                break;

            case 11: // +CPPP####### -> CCC (PP) ###-####
                country = value[0];
                city = value.slice(1, 4);
                number = value.slice(4);
                break;

            case 12: // +CCCPP####### -> CCC (PP) ###-####
                country = value.slice(0, 3);
                city = value.slice(3, 5);
                number = value.slice(5);
                break;

            default:
                return tel;
        }

        if (country == 1) {
            country = "";
        }

        number = number.slice(0, 3) + '-' + number.slice(3);

        return (country + " (" + city + ") " + number).trim();
    };
});

angular.module('admin.Filters').filter('defaultNull', function () {
    return function (value) {
        var blankValue = "---";

        if (!value)
            return blankValue;

        if(!value.toString().length)
            return blankValue;

        return value;
    };
});

angular.module('admin.Filters').filter('breakLines', function () {
    return function (value) {
        if (!value) return '';

        return value.replace(/(?:\r\n|\r|\n)/g, '<br />');
    };
});

angular.module('admin.Filters').filter('bytes', function () {
    return function (bytes, precision) {
        if (isNaN(parseFloat(bytes)) || !isFinite(bytes))
            return '-';

        if (typeof precision === 'undefined')
            precision = 1;

        var units = ['bytes', 'KB', 'MB', 'GB', 'TB', 'PB'],
			number = Math.floor(Math.log(bytes) / Math.log(1024));

        return (bytes / Math.pow(1024, Math.floor(number))).toFixed(precision) + ' ' + units[number];
    }
});